namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System;
    using MediatR;
    using Requests.WinLossByProduct;
    using Services;
    using ViewModels.WinLossByProduct;

    public class WinLossByProductHandler :
        WinLossByProductBaseHandler,
        IRequestHandler<WinLossByProductQuery, WinLossByProductViewModel>
    {
        public WinLossByProductHandler(
            WinLossByProductService reportService,
            ProductService productService,
            ProductDetailService productDetailService,
            CacheRefService cacheRefService)
            : base(reportService, productService, productDetailService, cacheRefService)
        {
        }

        public WinLossByProductViewModel Handle(WinLossByProductQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId, message.Site, message.CurrencyId, message.MinDate);
            var productDetails = productDetailService.GetProductDetails(message.CustId);

            var winLossByBettypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByProduct(winLossByBettypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByProductViewModel(
                    reportData,
                    grandTotal,
                    message,
                    winLossByBettypeParam,
                    BuildDisplayedBetTypeName,
                    BuildProductSelectListItems(winLossByBettypeParam.UserSelectedProductIds, products),
                    BuildProductDetailViewModels(winLossByBettypeParam.UserSelectedResourceIds, productDetails));
        }
    }
}