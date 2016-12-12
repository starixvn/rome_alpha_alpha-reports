namespace Alpha.Reports.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.RemotingRef;
    using Criteria.WinLoss;
    using DataAccesses;
    using Models;
    using Models.Winloss;
    using Params;

    public class WinLossServiceImpl : WinLossService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossDataAccess dataAccess;

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
        }

        public ReportData<WinLoss> GetWinLoss(WinLossParam param)
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

            var winLossData = dataAccess.GetWinLoss(criteria);            

            return winLossData;
        }

        public ReportData<EnteredSportResult> GetEnteredSportResults()
        {
            var enteredResults = new List<EnteredSportResult>();
            List<EnteredSportDaily> enteredSportDailies = dataAccess.GetEnteredSportDailies();

            enteredResults = enteredSportDailies.GroupBy(x => x.SportTypeId)
                .Select(y => new EnteredSportResult(
                    y.Key,
                    CacheRef.GetSportName(y.Key),
                    y.ToList().Where(x => x.IsToday).FirstOrDefault(),
                    y.ToList().Where(x => !x.IsToday).FirstOrDefault())).ToList();
        
            return new ReportData<EnteredSportResult>(enteredResults);
        }
    }
}
