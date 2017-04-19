namespace Alpha.Reports.Site.Handlers.WinLossAnalysisChart
{
    using Extensions;
    using MediatR;
    using Params;
    using Requests.WinLossAnalysisChart;
    using Services;
    using System;
    using ViewModels.WinLossAnalysisChart;

    public class WinLossAnalysisChartHandler :
        BaseHandler,
        IRequestHandler<WinLossAnalysisChartQuery, WinLossAnalysisChartViewModel>
    {
        private readonly ProductService productService;
        private readonly WinLossAnalysisChartService winLossAnalysisChartService;

        public WinLossAnalysisChartHandler(
            ProductService productService,
            WinLossAnalysisChartService winLossAnalysisChartService)
        {
            this.productService = productService;
            this.winLossAnalysisChartService = winLossAnalysisChartService;
        }

        public WinLossAnalysisChartViewModel Handle(WinLossAnalysisChartQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.CustId, message.UserSelectedProductIds.ToListInt());

            var products = productService.GetProductsForCustomer(message.CustId);

            var param = new WinLossAnalysisChartParam(
                message.CustId,
                message.CustLevel,
                message.FromDate,
                message.ToDate,
                validUserSelectedProductIds);

            var reportData = winLossAnalysisChartService.GetWinLossAnalysisChart(param);

            var viewModel = new WinLossAnalysisChartViewModel(
                reportData,
                message,
                param,
                BuildProductSelectListItems(param.UserSelectedProductIds, products));

            return viewModel;
        }
    }
}