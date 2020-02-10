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
                    param.BetCount);
            var tuple = dynamicRepository.FetchMultiple<BetChoiceMatchingModel, BetChoiceSharedIPModel>(criteria);
            var eventGroups = Group(tuple.Item1);
            return new Tuple<IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceSharedIPModel>>(eventGroups, tuple.Item2);
        }

        private static List<BetChoiceMatchingEventModel> Group(IEnumerable<BetChoiceMatchingModel> items)
        {
            var eventGroups = new List<BetChoiceMatchingEventModel>();
            foreach (var item in items)
            {
                var eventGroup = new BetChoiceMatchingEventModel(item);
                var selectedEventGroup = eventGroups.FirstOrDefault(g => g.Key == eventGroup.Key);
                if (selectedEventGroup == null)
                {
                    eventGroups.Add(eventGroup);
                }
                else
                {
                    selectedEventGroup.Add(item);
                }
            }

            return eventGroups;
        }

        public BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param)
            => dynamicRepository.Get<BetChoiceUplinesModel>(new BetChoiceUplinesCriteria(param.SearchAccountId));
    }
}
