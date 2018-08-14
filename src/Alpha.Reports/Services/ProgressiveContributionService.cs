namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.ProgressiveContribution;
    using DataAccesses;
    using Extensions;
    using Models.ProgressiveContribution;
    using Params;

    public interface ProgressiveContributionService
    {
        IEnumerable<ProgressiveContributionByProductModel> GetProgressiveContributions(ProgressiveContributionParam param);
    }

    public class ProgressiveContributionServiceImpl : ProgressiveContributionService
    {
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;
        private readonly ProgressiveContributionDataAccess progressiveContributionDataAccess;

        public ProgressiveContributionServiceImpl(
            ProgressiveContributionDataAccess progressiveContributionDataAccess,
            ReportDateService reportDateService,
            CacheRefService cacheRefService)
        {
            this.progressiveContributionDataAccess = progressiveContributionDataAccess;
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
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.UserSelectedProductIds,
                param.ReportLevel);

            var contributionByBetTypes = progressiveContributionDataAccess.GetProgressiveContributions(criteria);
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