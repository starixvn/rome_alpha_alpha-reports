namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System;
    using MediatR;
    using RemotingRef;
    using Requests.WinLossByBetType;
    using Services;
    using ViewModels.WinLossByBetType;

    public class WinLossByBetTypeHandler :
        WinLossByBetTypeBaseHandler,
        IRequestHandler<WinLossByBetTypeQuery, WinLossByBetTypeViewModel>
    {
        public WinLossByBetTypeHandler(
            WinLossByBetTypeService reportService, 
            ProductService productService, 
            CacheRefService cacheRefService) 
            : base(reportService, productService, cacheRefService)
        {
        }

        public WinLossByBetTypeViewModel Handle(WinLossByBetTypeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId);
            var winLossByBettypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByBetType(winLossByBettypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByBetTypeViewModel(
                    reportData, 
                    grandTotal, 
                    message, 
                    winLossByBettypeParam,
                    BuildDisplayedBetTypeName,
                    BuildProductSelectListItems(winLossByBettypeParam.UserSelectedProductIds, products),
                    cacheRefService);
        }
    }
}