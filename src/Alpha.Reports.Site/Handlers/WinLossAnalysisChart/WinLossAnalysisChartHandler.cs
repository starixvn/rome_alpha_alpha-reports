namespace Alpha.Reports.Site.Handlers.WinLossAnalysisChart
{
    using System;
    using Extensions;
    using MediatR;
    using Params;
    using Requests.WinLossAnalysisChart;
    using Services;
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

            var excludeProducts = productService.GetExcludeProductsForCustomer(message.CustId, message.MinDate);
            var products = productService.GetProductsForCustomer(message.CustId, message.MinDate, excludeProducts);

            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate,
                excludeProducts);
            var validUserSelectedExcludeProductIds = BuildValidUserSelectedExcludeProductIds(message.UserSelectedExcludeProductIds, excludeProducts);

            var param = new WinLossAnalysisChartParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                message.CustLevel,
                validUserSelectedExcludeProductIds);
            var reportData = winLossAnalysisChartService.GetWinLossAnalysisChart(param);
            var viewModel = new WinLossAnalysisChartViewModel(
                reportData,
                message,
                param,
                BuildProductSelectListItems(param.UserSelectedProductIds, products),
                BuildProductSelectListItems(validUserSelectedExcludeProductIds, excludeProducts));

            return viewModel;
        }
    }
}