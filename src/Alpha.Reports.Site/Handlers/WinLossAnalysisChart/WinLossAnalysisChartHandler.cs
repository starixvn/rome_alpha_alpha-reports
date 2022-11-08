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

            var products = productService.GetProductsForCustomer(message.ViewerId, message.Site, message.CurrencyId, message.MinDate);

            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);

            var param = new WinLossAnalysisChartParam(
                message.CustId,
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                message.CustLevel);
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