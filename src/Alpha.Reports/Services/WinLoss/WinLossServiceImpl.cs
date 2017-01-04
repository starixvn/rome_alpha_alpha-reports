namespace Alpha.Reports.Services.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLoss;
    using DataAccesses;
    using DataAccesses.WinLoss;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Winloss;
    using Params;
    using RemotingRef;

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
            var reportData = ConfiguredCacheService
                 .GetOrSet(
                 EnteredResultsCacheKey,
                 () => GetDataEnteredSportResults(),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return reportData;
        }

        public IEnumerable<WinLossModel> GetWinLoss(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!param.IsValid())
            {
                return Enumerable.Empty<WinLossModel>();
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

        internal IEnumerable<EnteredSportResult> GetDataEnteredSportResults()
        {
            var dailyEnteredSportData = dynamicRepository.Fetch<DailyEnteredSport>(new DailyEnteredSportCriteria());

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