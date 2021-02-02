namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private readonly IUser viewer;

        public WinLossDetailHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService reportService,
            IUser viewer) : base(productService, productDetailService, reportService)
        {
            this.viewer = viewer;
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var productDetails = productDetailService.GetProductDetails(message.CustId, message.Site, message.CurrencyId);

            var winLossDetailParam = CreateParameter(message, message.CustLevel, message.IsEnableSabaPromotion);
            var reportData = reportService.GetWinLossDetail(winLossDetailParam);
            var viewModel = new WinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                BuildBreadcrumbLevel(message),
                BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                BuildProductDetailViewModels(winLossDetailParam.UserSelectedResourceIds, productDetails));

            return viewModel;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            message.CustId = viewer.Id;
            var result = new List<string>();
            result.Add(Resources.Tooltip_NetTurnover);

            var liveCasinoTooltips = BuildTooltipsForLiveCasino(message.CustId, message.Site, message.CurrencyId);
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                result.Add(liveCasinoTooltips);
            }

            var productsNeedDelayTimeToSumarizeData = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.CustId, message.Site, message.CurrencyId).Select(product => (int)product);

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                result.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            return result;
        }

        private string BuildTooltipsForLiveCasino(int custId, string site, int currencyId)
        {            
            var offeredLiveCasino = productService.FilterProductsByCustomer(custId, site, currencyId, ProductIdConstants.ProductListForTooltipsLiveCasino);
            var liveCasinoNames = string.Join(", ", offeredLiveCasino.Select(i => i.Name));

            return offeredLiveCasino != null && offeredLiveCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_livecasino, liveCasinoNames) :
                string.Empty;
        }
    }
}