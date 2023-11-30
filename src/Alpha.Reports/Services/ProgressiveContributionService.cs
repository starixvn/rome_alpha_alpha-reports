namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
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
        private readonly IPartialUMService partialUMService;

        public ProgressiveContributionServiceImpl(
            ProgressiveContributionDataAccess progressiveContributionDataAccess,
            ReportDateService reportDateService,
            CacheRefService cacheRefService,
            IPartialUMService partialUMService)
        {
            this.progressiveContributionDataAccess = progressiveContributionDataAccess;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
            this.partialUMService = partialUMService;
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
                param.UserSelectedProductIds == null ? string.Empty : param.UserSelectedProductIds.JoinWithComma(),
                param.ReportLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

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