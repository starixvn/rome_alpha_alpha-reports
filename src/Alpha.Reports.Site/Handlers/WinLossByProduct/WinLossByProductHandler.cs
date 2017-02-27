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
        public WinLossByProductHandler(WinLossByProductService reportService, CacheRefService cacheRefService) 
            : base(reportService, cacheRefService)
        {
        }

        public WinLossByProductViewModel Handle(WinLossByProductQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var param = CreateWinLossByProductParamater(message);
            var reportData = reportService.GetWinLossByProduct(param);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByProductViewModel(
                    reportData, 
                    grandTotal, 
                    message,
                    param, 
                    BuildDisplayedProdutName, 
                    cacheRefService);
        }
    }
}