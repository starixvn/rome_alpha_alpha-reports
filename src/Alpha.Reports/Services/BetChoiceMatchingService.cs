namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Utilities.IP;
    using Alpha.Web.Helpers;
    using Fanex.BetList.Core.Constants;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;

    public interface BetChoiceMatchingService
    {
        Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>> Query(BetChoiceMatchingParam param);

        IEnumerable<BetChoiceMatchingEventModel> ExcelQuery(BetChoiceMatchingExcelParam param);

        BetChoiceMatchingUplinesModel QueryUplines(BetChoiceUplinesParam param);
    }

    public class BetChoiceMatchingServiceImpl : BetChoiceMatchingService
    {
        private const int usOddsTypeMultiply = 100;
        private const string oddsFormat = "{0:#,##0.###}";

        private static List<int> oddsBettypes = new List<int>
        {
            BetTypes.Handicap,
            BetTypes.OverUnder,
            BetTypes.FirstHalfHandicap,
            BetTypes.FirstHalfOverUnder
        };

        private readonly IDynamicRepository dynamicRepository;
        private readonly IUserSession userSession;
        private readonly IPService ipService;
        private readonly CacheRefService cacheRefService;
        private readonly PartialUMService partialUMService;

        public BetChoiceMatchingServiceImpl(
            IDynamicRepository dynamicRepository,
            IUserSession userSession,
            IPService ipService,
            CacheRefService cacheRefService,
            PartialUMService partialUMService)
            => (this.dynamicRepository, this.userSession, this.ipService, this.cacheRefService, this.partialUMService) = (dynamicRepository, userSession, ipService, cacheRefService, partialUMService);

        public Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>> Query(BetChoiceMatchingParam param)
        {
            var criteria = new BetChoiceMatchingCriteria(
                param.CustId,
                param.WinlostDate,
                param.MoveBetsDay,
                param.BetCount,
                param.IsExportExcel);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var tuple = dynamicRepository.FetchMultiple<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(criteria);
            var tickets = tuple.Item1;
            var vnIPs = userSession.IsAdminCanViewVNIP ? new List<string>() : BuildListVNIPs(tickets);
            foreach (var ticket in tickets)
            {
                CalculateDisplayOdds(ticket);
                if (!userSession.IsAdminCanViewVNIP && !string.IsNullOrEmpty(ticket.IP) && vnIPs.Contains(ticket.IP))
                {
                    ticket.HideIP();
                }
            }

            var events = tickets.GroupBy(ticket => ticket.EventKey).Select(eventTickets =>
            {
                var users = eventTickets.GroupBy(ticket => ticket.CustId).Select(userTickets => new BetChoiceMatchingUserModel(userTickets));
                return new BetChoiceMatchingEventModel(users);
            });

            var ips = tuple.Item2;
            if (!userSession.IsAdminCanViewVNIP)
            {
                ips = ips.Where(ip => !vnIPs.Contains(ip.IP));
            }

            return new Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>>(events, ips);
        }

        private void CalculateDisplayOdds(BetChoiceMatchingTicketModel ticket)
        {
            var isShowOddsType = oddsBettypes.Contains(ticket.BetType) && ticket.OddsType > 0;
            var oddsValue = ticket.Odds;
            if (isShowOddsType)
            {
                if (ticket.OddsType == OddsTypes.Decimal)
                {
                    oddsValue = oddsValue + 1;
                }
                else
                {
                    if (ticket.OddsType == OddsTypes.US)
                    {
                        oddsValue = oddsValue * usOddsTypeMultiply;
                    }
                }
            }

            ticket.Odds = oddsValue;
            ticket.DisplayOdds = string.Format(oddsFormat, ticket.Odds);
            ticket.DisplayOddsType = isShowOddsType
                    ? cacheRefService.GetOddsTypeName(ticket.OddsType.ToString())
                    : string.Empty;
        }

        private IEnumerable<string> BuildListVNIPs(IEnumerable<BetChoiceMatchingTicketModel> tickets)
        {
            var vnIPs = new List<string>();
            var nonVNIPs = new List<string>();
            foreach (var ticket in tickets)
            {
                var ip = ticket.IP;
                if (string.IsNullOrEmpty(ip) || vnIPs.Contains(ip) || nonVNIPs.Contains(ip))
                {
                    continue;
                }

                if (ipService.IsVNIP(ip))
                {
                    vnIPs.Add(ip);
                }
                else
                {
                    nonVNIPs.Add(ip);
                }
            }

            return vnIPs;
        }

        public BetChoiceMatchingUplinesModel QueryUplines(BetChoiceUplinesParam param)
        {
            var criteria = new BetChoiceUplinesCriteria(param.SearchAccountId);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return dynamicRepository.Get<BetChoiceMatchingUplinesModel>(criteria);
        }

        public IEnumerable<BetChoiceMatchingEventModel> ExcelQuery(BetChoiceMatchingExcelParam param)
        {
            var tuple = Query(param);
            var events = tuple.Item1;
            var filterEvents = new List<BetChoiceMatchingEventModel>();
            foreach (var eventModel in events)
            {
                var filterUsers = eventModel.Users;
                if (!string.IsNullOrEmpty(param.Username))
                {
                    var usernames = param.Username.Split(',').Select(x => x.ToLowerInvariant());
                    filterUsers = filterUsers.Where(user => usernames.Contains(user.Username.ToLowerInvariant())).ToList();
                }

                if (param.Stake.HasValue)
                {
                    filterUsers = filterUsers.Where(user => user.Stake >= param.Stake.Value).ToList();
                }

                if (param.MemberWinLossValue.HasValue)
                {
                    switch (param.MemberWinLossOperator)
                    {
                        case MemberWinLossOperators.GreatorOrEqual:
                            filterUsers = filterUsers.Where(user => !user.IsRunning && user.MemberWinLoss >= param.MemberWinLossValue.Value).ToList();
                            break;
                        case MemberWinLossOperators.Equal:
                            filterUsers = filterUsers.Where(user => !user.IsRunning && user.MemberWinLoss == param.MemberWinLossValue.Value).ToList();
                            break;
                        default:
                            filterUsers = filterUsers.Where(user => !user.IsRunning && user.MemberWinLoss <= param.MemberWinLossValue.Value).ToList();
                            break;
                    }
                }

                if (filterUsers.Any())
                {
                    filterEvents.Add(new BetChoiceMatchingEventModel(filterUsers));
                }
            }

            return filterEvents;
        }
    }
}
