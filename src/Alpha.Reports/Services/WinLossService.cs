namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
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

        // Async methods
        Task<EnteredSportResult> GetTotalEnteredSportYesterdayAndTodayAsync();

        EnteredSportResult GetTotalEnteredSportYesterdayAndToday(IEnumerable<EnteredSportResult> dailyEnteredSportData);

        Task<IEnumerable<EnteredSportResult>> GetEnteredSportResultsAsync();
    }

    public class WinLossServiceImpl : WinLossService
    {
        private const int EnteredResultsCacheInMinutes = 1;
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
            var reportData = cacheService
                 .GetOrSet(
                 EnteredResultsCacheKey,
                 () => GetDataEnteredSportResults(),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(EnteredResultsCacheInMinutes)));

            return reportData;
        }

        public IEnumerable<WinLossModel> GetWinLoss(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.ViewerId, param.Site, param.CurrencyId, param.UserSelectedProductIds);
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

        internal IEnumerable<EnteredSportResult> GetDataEnteredSportResults()
        {
            var criteria = new DailyEnteredSportCriteria();

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

        // Async methods
        public async Task<EnteredSportResult> GetTotalEnteredSportYesterdayAndTodayAsync()
            => GetTotalEnteredSportYesterdayAndToday(await GetEnteredSportResultsAsync());

        public EnteredSportResult GetTotalEnteredSportYesterdayAndToday(IEnumerable<EnteredSportResult> dailyEnteredSportData)
        {
            int totalEnteredToday = 0,
                totalNonEnteredToday = 0,
                totalEnteredYesterday = 0,
                totalNonEnteredYesterday = 0;

            foreach (var data in dailyEnteredSportData)
            {
                if (data.EnteredSportToday != null)
                {
                    totalEnteredToday += data.EnteredSportToday.Entered;
                    totalNonEnteredToday += data.EnteredSportToday.NonEntered;
                }

                if (data.EnteredSportYesterday != null)
                {
                    totalEnteredYesterday += data.EnteredSportYesterday.Entered;
                    totalNonEnteredYesterday += data.EnteredSportYesterday.NonEntered;
                }
            }

            var totalToday = new DailyEnteredSport(totalEnteredToday, totalNonEnteredToday);
            var totalYesterday = new DailyEnteredSport(totalEnteredYesterday, totalNonEnteredYesterday);

            return new EnteredSportResult(0, string.Empty, totalToday, totalYesterday);
        }

        public Task<IEnumerable<EnteredSportResult>> GetEnteredSportResultsAsync()
            => cacheService.GetOrSetAsync(
                 EnteredResultsCacheKey,
                 () => GetDataEnteredSportResults(),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(EnteredResultsCacheInMinutes)));
    }
}