namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;

    public interface BetChoiceMatchingService
    {
        Tuple<IEnumerable<BetChoiceMatchingModel>, IEnumerable<BetChoiceSharedIPModel>> Query(BetChoiceMatchingParam param);

        BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param);
    }

    public class BetChoiceMatchingServiceImpl : BetChoiceMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetChoiceMatchingServiceImpl(IDynamicRepository dynamicRepository)
            => (this.dynamicRepository) = (dynamicRepository);

        public Tuple<IEnumerable<BetChoiceMatchingModel>, IEnumerable<BetChoiceSharedIPModel>> Query(BetChoiceMatchingParam param)
            => dynamicRepository.FetchMultiple<BetChoiceMatchingModel, BetChoiceSharedIPModel>(
                new BetChoiceMatchingCriteria(
                    param.CustId,
                    param.WinlostDate,
                    param.MoveBetsDay,
                    param.BetCount));

        public BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param)
            => dynamicRepository.Get<BetChoiceUplinesModel>(new BetChoiceUplinesCriteria(param.SearchAccountId));
    }
}
