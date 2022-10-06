namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.PartialUM.Services;
    using Configurations;
    using Criteria.WinLoss;
    using DataAccesses;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Winloss;
    using Params;

    public interface WinLossService
    {
        IEnumerable<EnteredSportResult> GetEnteredSportResults();

        IEnumerable<WinLossModel> GetWinLoss(WinLossParam param);
    }

    public class WinLossServiceImpl : WinLossService
    {
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;
        private readonly WinLossDataAccess dataAccess;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly ICacheService cacheService;
        private readonly IPartialUMService partialUMService;

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDataAccess dataAccess,
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService,
            ICacheService cacheService,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.dataAccess = dataAccess;
            this.cacheRefService = cacheRefService;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<EnteredSportResult> GetEnteredSportResults()
        {
            bool includeTestData = SiteSettings.LocalTest;

            var reportData = cacheService
                 .GetOrSet(
                 EnteredResultsCacheKey,
                 () => GetDataEnteredSportResults(includeTestData),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));

            return reportData;
        }

        public IEnumerable<WinLossModel> GetWinLoss(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.CustId, param.Site, param.CurrencyId, param.UserSelectedProductIds);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, param.UserSelectedResourceIds);

            var criteria = new WinLossCriteria(
                param.CustId,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel,
                param.ProductDetailsXML);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var winlossData = dataAccess.GetWinLoss(criteria);

            return winlossData;
        }

        internal IEnumerable<EnteredSportResult> GetDataEnteredSportResults(bool includeTestData)
        {
            var criteria = new DailyEnteredSportCriteria(includeTestData);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var dailyEnteredSportData = dynamicRepository.Fetch<DailyEnteredSport>(criteria);

            if (dailyEnteredSportData.Any())
            {
                return dailyEnteredSportData.GroupBy(dailyEnteredSport => dailyEnteredSport.SportType)
                    .Select(dailyEnteredSports => new EnteredSportResult(
                    dailyEnteredSports.Key,
                    cacheRefService.GetSportName(dailyEnteredSports.Key),
                    dailyEnteredSports.FirstOrDefault(enteredSportResult => enteredSportResult.IsToday),
                    dailyEnteredSports.FirstOrDefault(enteredSportResult => !enteredSportResult.IsToday)));
            }

            return Enumerable.Empty<EnteredSportResult>();
        }
    }
}