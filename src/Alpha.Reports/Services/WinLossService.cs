namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Criteria.WinLoss;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Winloss;
    using Params;
    using RemotingRef;

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
        private readonly WinLossDataAccess dataAccess;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDataAccess dataAccess,
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<EnteredSportResult> GetEnteredSportResults()
        {
            bool includeTestData = SiteSettings.LocalTest;

            var reportData = ConfiguredCacheService
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
                productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds);

            var criteria = new WinLossCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel);

            var winlossData = dataAccess.GetWinLoss(criteria);

            return winlossData;
        }

        internal IEnumerable<EnteredSportResult> GetDataEnteredSportResults(bool includeTestData)
        {
            var dailyEnteredSportData = dynamicRepository.Fetch<DailyEnteredSport>(new DailyEnteredSportCriteria(includeTestData));

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