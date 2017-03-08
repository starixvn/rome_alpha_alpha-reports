namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.WinLossAnalysis;
    using DataAccesses;
    using Models.WinLossAnalysis;
    using Params;

    public interface WinLossAnalysisService
    {
        IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisParam param);
    }

    public class WinLossAnalysisServiceImpl : WinLossAnalysisService
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossAnalysisDataAccess dataAccess;

        public WinLossAnalysisServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossAnalysisDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
        }

        public IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds);

            var criteria = new WinLossAnalysisCriteria(
                param.CustId,
                param.ReportTypeId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.CustLevel);

            var winlossAnalysisData = dataAccess.GetWinLossAnalysis(criteria);

            return winlossAnalysisData;
        }
    }
}