namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.ProgressiveContribution;
    using Fanex.Data.Repository;
    using Models.ProgressiveContribution;
    using Params;
    using RemotingRef;

    public interface ProgressiveContributionService
    {
        IEnumerable<ProgressiveContributionByProductModel> GetProgressiveContributions(ProgressiveContributionParam param);
    }

    public class ProgressiveContributionServiceImpl : ProgressiveContributionService
    {
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;
        private readonly IDynamicRepository dynamicRepository;

        public ProgressiveContributionServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<ProgressiveContributionByProductModel> GetProgressiveContributions(ProgressiveContributionParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new ProgressiveContributionCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.UserSelectedProductIds,
                param.ReportLevel);

            var contributionByBetTypes = dynamicRepository.Fetch<ProgressiveContributionByBetTypeModel>(criteria);
            var result = new List<ProgressiveContributionByProductModel>();

            if (contributionByBetTypes.Any())
            {
                var contributionByProducts = contributionByBetTypes.GroupBy(contribution => contribution.ProductId);

                foreach (var contribution in contributionByProducts)
                {
                    result.Add(new ProgressiveContributionByProductModel(
                        contribution.Key,
                        cacheRefService.GetProductName(contribution.Key),
                        contribution.ToList()));
                }
            }

            return result;
        }
    }
}