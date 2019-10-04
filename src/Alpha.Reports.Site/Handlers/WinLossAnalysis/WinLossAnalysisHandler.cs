namespace Alpha.Reports.Site.Handlers.WinLossAnalysis
{
    using System;
    using Extensions;
    using MediatR;
    using Requests.WinLossAnalysis;
    using Services;
    using ViewModels.WinLossAnalysis;

    public class WinLossAnalysisHandler :
        WinLossAnalysisBaseHandler,
        IRequestHandler<WinLossAnalysisQuery, WinLossAnalysisViewModel>
    {
        public WinLossAnalysisHandler(
            ProductService productService,
            WinLossAnalysisService reportService) : base(productService, reportService)
        {
        }

        public WinLossAnalysisViewModel Handle(WinLossAnalysisQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var excludeProducts = productService.GetExcludeProductsForCustomer(message.CustId, message.MinDate);
            var products = productService.GetProductsForCustomer(message.CustId, message.MinDate, excludeProducts);

            var winLossAnalysisParam = CreateWinLossAnalysisParamater(message, excludeProducts);
            var reportData = reportService.GetWinLossAnalysis(winLossAnalysisParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var viewModel = new WinLossAnalysisViewModel(
                reportData,
                grandTotal,
                message,
                winLossAnalysisParam,
                BuildProductSelectListItems(winLossAnalysisParam.UserSelectedProductIds, products));

            return viewModel;
        }
    }
}