namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Site.Features.SuperWinLossDetailByPT.Requests;
    using Alpha.Reports.Site.Features.SuperWinLossDetailByPT.ViewModels;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Web.Helpers;
    using App_GlobalResources;
    using MediatR;
    using Services;

    public class SuperWinLossDetailByPTHandler
        : SuperWinLossDetailByPTBaseHandler,
        IRequestHandler<SuperWinLossDetailByPTQuery, SuperWinLossDetailByPTViewModel>,
        IRequestHandler<ReportLegendQuery, ReportLegendViewModel>
    {
        private readonly BrowserHelper browserHelper;

        public SuperWinLossDetailByPTHandler(
            CacheRefService refService,
            ProductService productService,
            ProductDetailService productDetailService,
            SuperWinLossDetailByPTService reportService,
            BrowserHelper browserHelper)
            : base(refService, productService, productDetailService, reportService)
        {
            this.browserHelper = browserHelper;
        }

        public SuperWinLossDetailByPTViewModel Handle(SuperWinLossDetailByPTQuery message)
        {
            var products = productService.GetProductsForCustomer(message.CustId, message.Site, message.CurrencyId, message.MinDate);
            var productDetails = productDetailService.GetProductDetails(message.CustId, message.Site, message.CurrencyId);

            var superPTParam = CreateSuperPTParam(message.ViewerId);
            var winLossDetailByPTParam = CreateSuperWinLossDetailByPTParam(message);

            var positionTakings = reportService.GetSuperPositionTakings(superPTParam);
            var winLossDetailByProducts = reportService.GetSuperWinLossDetailByProducts(winLossDetailByPTParam);

            var reportData = GenerateReportData(positionTakings, winLossDetailByProducts, message.SortPropertyName, message.IsDescendingSort);
            var grandTotal = AccumulateGrandTotal(reportData);
            var ptProducts = GeneratePtProducts(positionTakings, products);

            var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

            return new SuperWinLossDetailByPTViewModel(
                reportData,
                grandTotal,
                message,
                winLossDetailByPTParam,
                ptProducts,
                BuildProductSelectListItems(winLossDetailByPTParam.UserSelectedProductIds, products),
                BuildProductDetailViewModels(winLossDetailByPTParam.UserSelectedResourceIds, productDetails),
                isOnlyMobile);
        }

        public ReportLegendViewModel Handle(ReportLegendQuery message)
        {
            var result = new List<string>
            {
                Resources.Tooltip_NetTurnover
            };

            var liveCasinoTooltips = BuildTooltipsForLiveCasino(message.CustId, message.Site, message.CurrencyId);
            if (!string.IsNullOrEmpty(liveCasinoTooltips))
            {
                result.Add(liveCasinoTooltips);
            }

            var rngCasinoTooltips = BuildTooltipsForRNGCasino(message.CustId, message.Site, message.CurrencyId);
            if (!string.IsNullOrWhiteSpace(rngCasinoTooltips))
            {
                result.Add(rngCasinoTooltips);
            }

            var productsNeedDelayTimeToSumarizeData = productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(
                message.CustId,
                message.Site,
                message.CurrencyId)
                .Select(product => (int)product);

            if (productsNeedDelayTimeToSumarizeData.Any())
            {
                result.Add(string.Format(
                    Resources.DelayIssueLegend,
                    string.Join(", ", productService.GetProductNames(productsNeedDelayTimeToSumarizeData))));
            }

            return new ReportLegendViewModel(result, GetGrossCommNotes(message.ViewerLevel));
        }

        private string BuildTooltipsForLiveCasino(int custId, string site, int currencyId)
        {
            var offeredLiveCasino = productService.FilterProductsByCustomer(custId, site, currencyId, ProductIdConstants.ProductListForTooltipsLiveCasino);
            var liveCasinoNames = string.Join(", ", offeredLiveCasino.Select(i => i.Name));

            return offeredLiveCasino != null && offeredLiveCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_livecasino, liveCasinoNames) :
                string.Empty;
        }

        private string BuildTooltipsForRNGCasino(int custId, string site, int currencyId)
        {
            var offeredRNGCasino = productService.FilterProductsByCustomer(custId, site, currencyId, ProductIdConstants.ProductListForTooltipsRNGCasino);
            var rngCasinoNames = string.Join(", ", offeredRNGCasino.Select(i => i.DisplayName));

            return offeredRNGCasino != null && offeredRNGCasino.Any() ?
                string.Format(Resources.Tooltip_NetTurnover_RNGCasino, rngCasinoNames) :
                string.Empty;
        }
    }
}