namespace Alpha.Reports.Site.Handlers.SuperWinLossDetailByPT
{
    using Alpha.Web.Helpers;
    using MediatR;
    using Requests.SuperWinLossDetailByPT;
    using Services;
    using ViewModels.SuperWinLossDetailByPT;

    public class SuperWinLossDetailByPTHandler
        : SuperWinLossDetailByPTBaseHandler,
        IRequestHandler<SuperWinLossDetailByPTQuery, SuperWinLossDetailByPTViewModel>
    {
        private readonly BrowserHelper browserHelper;

        public SuperWinLossDetailByPTHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            SuperWinLossDetailByPTService reportService,
            BrowserHelper browserHelper)
            : base(productService, productDetailService, reportService)
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
    }
}