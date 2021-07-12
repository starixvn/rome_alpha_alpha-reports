namespace Alpha.Reports.Site.Handlers.SuperWinLossDetailByPT
{
    using MediatR;
    using Requests.SuperWinLossDetailByPT;
    using Services;
    using ViewModels.SuperWinLossDetailByPT;

    public class SuperWinLossDetailByPTHandler
        : SuperWinLossDetailByPTBaseHandler,
        IRequestHandler<SuperWinLossDetailByPTQuery, SuperWinLossDetailByPTViewModel>
    {
        public SuperWinLossDetailByPTHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            SuperWinLossDetailByPTService reportService)
            : base(productService, productDetailService, reportService)
        {
        }

        public SuperWinLossDetailByPTViewModel Handle(SuperWinLossDetailByPTQuery message)
        {
            var products = productService.GetProductsForCustomer(message.CustId, message.Site, message.CurrencyId, message.MinDate);
            var productDetails = productDetailService.GetProductDetails(message.CustId, message.Site, message.CurrencyId);

            var superPTParam = CreateSuperPTParam(message.ViewerId);
            var winLossDetailByPTParam = CreateSuperWinLossDetailByPTParam(message);

            var positionTakings = reportService.GetSuperPositionTakings(superPTParam);
            var winLossDetailByProducts = reportService.GetSuperWinLossDetailByProducts(winLossDetailByPTParam);

            var reportData = GenerateReportData(positionTakings, winLossDetailByProducts);
            var grandTotal = AccumulateGrandTotal(reportData);
            var ptProducts = GeneratePtProducts(positionTakings, products);

            return new SuperWinLossDetailByPTViewModel(
                reportData,
                grandTotal,
                message,
                winLossDetailByPTParam,
                ptProducts,
                BuildProductSelectListItems(winLossDetailByPTParam.UserSelectedProductIds, products),
                BuildProductDetailViewModels(winLossDetailByPTParam.UserSelectedResourceIds, productDetails));
        }
    }
}