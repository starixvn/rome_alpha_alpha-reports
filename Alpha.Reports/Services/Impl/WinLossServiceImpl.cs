namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLoss;
    using DataAccesses;
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

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDataAccess dataAccess,
            IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
        }

        public IEnumerable<WinLoss> GetWinLoss(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!param.IsValid())
            {
                return Enumerable.Empty<WinLoss>();
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

        public IEnumerable<EnteredSportResult> GetEnteredSportResults()
        {
            var reportData = CacheService
                 .GetOrSet(
                 EnteredResultsCacheKey,
                 () => GetDataEnteredSportResults(),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return reportData;
        }

        private IEnumerable<EnteredSportResult> GetDataEnteredSportResults()
        {
            var dailyEnteredSportData = dynamicRepository.Fetch<DailyEnteredSport>(new DailyEnteredSportCriteria());

            var enteredResultData = dailyEnteredSportData.GroupBy(x => x.SportType)
                    .Select(y => new EnteredSportResult(
                    y.Key,
                    CacheRefFactory.CacheRef.GetSportName(y.Key),
                    y.ToList().Where(x => x.IsToday).FirstOrDefault(),
                    y.ToList().Where(x => !x.IsToday).FirstOrDefault())).OrderBy(x => x.SportName).ToList();
        
            return enteredResultData;
        }
    }
}
