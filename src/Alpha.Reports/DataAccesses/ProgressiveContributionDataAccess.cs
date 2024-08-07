namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.ProgressiveContribution;
    using Fanex.Data.Repository;
    using Models.ProgressiveContribution;
    using Services;

    public interface ProgressiveContributionDataAccess
    {
        IEnumerable<ProgressiveContributionByBetTypeModel> GetProgressiveContributions(ProgressiveContributionCriteria criteria);
    }

    public class ProgressiveContributionDataAccessImpl : ProgressiveContributionDataAccess
    {
        private readonly IDynamicRepository repository;
        private readonly CacheRefService cacheRefService;

        public ProgressiveContributionDataAccessImpl(
            IDynamicRepository repository,
            CacheRefService cacheRefService)
        {
            this.repository = repository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<ProgressiveContributionByBetTypeModel> GetProgressiveContributions(ProgressiveContributionCriteria criteria)
        {
            var progressiveContributions = repository.Fetch<ProgressiveContributionByBetTypeModel>(criteria);

            foreach (var item in progressiveContributions)
            {
                item.BetTypeName = cacheRefService.GetBetTypeName(item.BetTypeId);
            }

            return progressiveContributions;
        }
    }
}