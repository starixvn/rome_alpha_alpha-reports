namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface BetChoiceMatchingService
    {
        Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>> Query(BetChoiceMatchingParam param);

        IEnumerable<BetChoiceMatchingEventModel> ExcelQuery(BetChoiceMatchingExcelParam param);

        BetChoiceMatchingUplinesModel QueryUplines(BetChoiceUplinesParam param);
    }

    public class BetChoiceMatchingServiceImpl : BetChoiceMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IUserSession userSession;
        private readonly IPService ipService;

        public BetChoiceMatchingServiceImpl(IDynamicRepository dynamicRepository, IUserSession userSession, IPService ipService)
            => (this.dynamicRepository, this.userSession, this.ipService) = (dynamicRepository, userSession, ipService);

        public Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>> Query(BetChoiceMatchingParam param)
        {
            var criteria = new BetChoiceMatchingCriteria(
                    param.CustId,
                    param.WinlostDate,
                    param.MoveBetsDay,
                    param.BetCount,
                    param.IsExportExcel);
            var tuple = dynamicRepository.FetchMultiple<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(criteria);
            var tickets = tuple.Item1;
            var vnIPs = userSession.IsAdminCanViewVNIP ? new List<string>() : BuildListVNIPs(tickets);
            var events = tickets.GroupBy(ticket => ticket.EventKey).Select(eventTickets =>
            {
                CheckToHideIP(eventTickets, vnIPs);
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

        private void CheckToHideIP(IEnumerable<BetChoiceMatchingTicketModel> tickets, IEnumerable<string> vnIPs)
        {
            if (!userSession.IsAdminCanViewVNIP)
            {
                foreach (var ticket in tickets)
                {
                    if (!string.IsNullOrEmpty(ticket.IP) && vnIPs.Contains(ticket.IP))
                    {
                        ticket.HideIP();
                    }
                }
            }
        }

        public BetChoiceMatchingUplinesModel QueryUplines(BetChoiceUplinesParam param)
            => dynamicRepository.Get<BetChoiceMatchingUplinesModel>(new BetChoiceUplinesCriteria(param.SearchAccountId));

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
                    var usernames = param.Username.Split(',');
                    filterUsers = filterUsers.Where(user => usernames.Contains(user.Username)).ToList();
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
