namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLoss;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Models.Winloss;
    using Params;
    using RemotingRef;

    public class WinLossServiceImpl : WinLossService
    {
        private const int EnteredResultsCacheTimeInMinutes = 5;
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

        public ReportData<WinLoss> GetWinLosses(WinLossParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (!param.IsValid())
            {
                return new ReportData<WinLoss>(Enumerable.Empty<WinLoss>(), 0, new WinLoss());
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

            var winlossess = dataAccess.GetWinLosses(criteria).OrderBy(x => x.CustName).ToList();
           
            if(winlossess.Count() > 0)
            {
                var totalWinLoss = BuildGrandTotal(winlossess, param.ReportLevel);
                return new ReportData<WinLoss>(winlossess, winlossess.Count, totalWinLoss);
            }

            return new ReportData<WinLoss>(Enumerable.Empty<WinLoss>(), 0, new WinLoss());
        }

        public ReportData<EnteredSportResult> GetEnteredSportResults()
        {
            var reportData = CacheService
                 .GetOrSet(
                 EnteredResultsCacheKey,
                 () => GetDatabaseEnteredSportResults(),
                 new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return reportData;
        }

        private ReportData<EnteredSportResult> GetDatabaseEnteredSportResults()
        {
            var enteredResults = new List<EnteredSportResult>();
            IEnumerable<EnteredSportDaily> enteredSportDailies = GetEnteredSportDailies();

            enteredResults = enteredSportDailies.GroupBy(x => x.SportType)
                    .Select(y => new EnteredSportResult(
                    y.Key,
                    CacheRef.GetSportName(y.Key),
                    y.ToList().Where(x => x.IsToday).FirstOrDefault(),
                    y.ToList().Where(x => !x.IsToday).FirstOrDefault())).OrderBy(x => x.SportName).ToList();
        
            return new ReportData<EnteredSportResult>(enteredResults);
        }

        private IEnumerable<EnteredSportDaily> GetEnteredSportDailies()
        {
            IEnumerable<EnteredSportDaily> enteredSportDailies = dynamicRepository.Fetch<EnteredSportDaily>(new EnteredSportDailyCriteria());

            return enteredSportDailies;
        }

        internal virtual WinLoss BuildGrandTotal(List<WinLoss> winlossess, UserLevel reportLevel)
        {
            WinLoss grandTotal;
            int i;

            for (i = 1, grandTotal = winlossess[0]; i < winlossess.Count(); i++)
            {
                grandTotal += winlossess[i];
            }

            return grandTotal;
        }
    }
}
