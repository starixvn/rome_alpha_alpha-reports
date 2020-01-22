namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface BetChoiceMatchingService
    {
        IEnumerable<BetChoiceMatchingModel> Query(BetChoiceMatchingParam param);
    }

    public class BetChoiceMatchingServiceImpl: BetChoiceMatchingService
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
    }
}
