namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
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

            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossAnalysisParam = CreateWinLossAnalysisParamater(message);
            var reportData = reportService.GetWinLossinLossAnalysis(winLossAnalysisParam);
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