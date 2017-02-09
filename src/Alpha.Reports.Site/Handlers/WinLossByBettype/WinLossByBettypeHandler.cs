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
        private readonly ReportDateService reportDateService;

        public WinLossByBetTypeHandler(WinLossByBetTypeService reportService, ReportDateService reportDateService, CacheRefService cacheRefService) 
            : base(reportService, cacheRefService)
        {
            this.reportDateService = reportDateService;
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

            return new WinLossByBetTypeViewModel(reportData, grandTotal, message, winLossByBettypeParam, reportDateService.GetBalanceUpDay(), BuildDisplayedBetTypeName);
        }
    }
}