namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Services;
    using Criteria.WinLossAnalysis;
    using DataAccesses;
    using Extensions;
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
        private readonly IPartialUMService partialUMService;

        public WinLossAnalysisServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            WinLossAnalysisDataAccess dataAccess,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds =
                productService.FilterProductIds(param.Site, param.CurrencyId, param.UserSelectedProductIds);

            var criteria = new WinLossAnalysisCriteria(
                param.CustId,
                param.ReportTypeId,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds == null ? string.Empty : validUserSelectedProductIds.JoinWithComma(),
                param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return dataAccess.GetWinLossAnalysis(criteria);
        }
    }
}