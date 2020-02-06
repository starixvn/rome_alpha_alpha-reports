namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface BetChoiceMatchingService
    {
        IEnumerable<BetChoiceMatchingModel> Query(BetChoiceMatchingParam param);

        BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param);
    }

    public class BetChoiceMatchingServiceImpl : BetChoiceMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetChoiceMatchingServiceImpl(IDynamicRepository dynamicRepository)
            => (this.dynamicRepository) = (dynamicRepository);

        public IEnumerable<BetChoiceMatchingModel> Query(BetChoiceMatchingParam param)
            => dynamicRepository.Fetch<BetChoiceMatchingModel>(
                new BetChoiceMatchingCriteria(
                    param.CustId,
                    param.WinlostDate,
                    param.MoveBetsDay,
                    param.BetCount));

        public BetChoiceUplinesModel QueryUplines(BetChoiceUplinesParam param)
            => dynamicRepository.Get<BetChoiceUplinesModel>(new BetChoiceUplinesCriteria(param.SearchAccountId));
    }
}
