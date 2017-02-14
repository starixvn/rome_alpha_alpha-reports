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
        public WinLossByBetTypeHandler(WinLossByBetTypeService reportService, CacheRefService cacheRefService) 
            : base(reportService, cacheRefService)
        {
        }

        public WinLossByBetTypeViewModel Handle(WinLossByBetTypeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByBettypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByBetType(winLossByBettypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByBetTypeViewModel(
                    reportData, 
                    grandTotal, 
                    message, 
                    winLossByBettypeParam, 
                    BuildDisplayedBetTypeName, 
                    cacheRefService);
        }
    }
}