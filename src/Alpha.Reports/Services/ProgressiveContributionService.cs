namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Criteria.ProgressiveContribution;
    using DataAccesses;
    using Extensions;
    using Fanex.Data.Repository;
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
        private readonly IPartialUMService partialUMService;
        private readonly IDynamicRepository repository;

        public ProgressiveContributionServiceImpl(
            ReportDateService reportDateService,
            CacheRefService cacheRefService,
            IPartialUMService partialUMService,
            IDynamicRepository repository)
        {
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
            this.partialUMService = partialUMService;
            this.repository = repository;
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

            var contributionByBetTypes = repository.Fetch<ProgressiveContributionByBetTypeModel>(criteria);
            var result = new List<ProgressiveContributionByProductModel>();

            if (contributionByBetTypes.Any())
            {
                contributionByBetTypes = AssignBetTypeName(contributionByBetTypes);
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

        private IEnumerable<ProgressiveContributionByBetTypeModel> AssignBetTypeName(
            IEnumerable<ProgressiveContributionByBetTypeModel> rawData)
        {
            foreach (var item in rawData)
            {
                item.BetTypeName = cacheRefService.GetBetTypeName(item.BetType);
                yield return item;
            }
        }
    }
}