namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System;
    using MediatR;
    using RemotingRef;
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
            CacheRefService cacheRefService)
            : base(reportService, productService, cacheRefService)
        {
        }

        public WinLossByProductViewModel Handle(WinLossByProductQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId, message.MinDate);
            var winLossByBettypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByProduct(winLossByBettypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByProductViewModel(
                    reportData,
                    grandTotal,
                    message,
                    winLossByBettypeParam,
                    BuildDisplayedBetTypeName,
                    BuildProductSelectListItems(winLossByBettypeParam.UserSelectedProductIds, products));
        }
    }
}