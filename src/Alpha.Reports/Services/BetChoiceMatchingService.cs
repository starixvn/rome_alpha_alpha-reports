namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface BetChoiceMatchingService
    {
        Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceSharedIPModel>> Query(BetChoiceMatchingParam param);

        IEnumerable<BetChoiceMatchingEventModel> ExcelQuery(BetChoiceMatchingExcelParam param);

        BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param);
    }

    public class BetChoiceMatchingServiceImpl : BetChoiceMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetChoiceMatchingServiceImpl(IDynamicRepository dynamicRepository)
            => (this.dynamicRepository) = (dynamicRepository);

        public Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceSharedIPModel>> Query(BetChoiceMatchingParam param)
        {
            var criteria = new BetChoiceMatchingCriteria(
                    param.CustId,
                    param.WinlostDate,
                    param.MoveBetsDay,
                    param.BetCount,
                    param.IsExportExcel);
            var tuple = dynamicRepository.FetchMultiple<BetChoiceMatchingTicketModel, BetChoiceSharedIPModel>(criteria);
            var events = Group(tuple.Item1);
            return new Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceSharedIPModel>>(events, tuple.Item2);
        }

        private static List<BetChoiceMatchingEventModel> Group(IEnumerable<BetChoiceMatchingTicketModel> tickets)
        {
            var events = new List<BetChoiceMatchingEventModel>();
            foreach (var ticket in tickets)
            {
                var eventModel = new BetChoiceMatchingEventModel(ticket);
                var selectedEventGroup = events.FirstOrDefault(g => g.Key == eventModel.Key);
                if (selectedEventGroup == null)
                {
                    events.Add(eventModel);
                }
                else
                {
                    selectedEventGroup.AddTicket(ticket);
                }
            }

            return events;
        }

        public BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param)
            => dynamicRepository.Get<BetChoiceUplinesModel>(new BetChoiceUplinesCriteria(param.SearchAccountId));

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
                            filterUsers = filterUsers.Where(user => user.MemberWinLoss >= param.MemberWinLossValue.Value).ToList();
                            break;
                        case MemberWinLossOperators.Equal:
                            filterUsers = filterUsers.Where(user => user.MemberWinLoss == param.MemberWinLossValue.Value).ToList();
                            break;
                        default:
                            filterUsers = filterUsers.Where(user => user.MemberWinLoss <= param.MemberWinLossValue.Value).ToList();
                            break;
                    }
                }

                if (filterUsers.Any())
                {
                    var firstItem = filterUsers.First().Tickets.First();
                    filterEvents.Add(new BetChoiceMatchingEventModel(firstItem, filterUsers));
                }
            }

            return filterEvents;
        }
    }
}
