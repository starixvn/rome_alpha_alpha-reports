namespace Alpha.Reports.Site.Handlers.WinLossByBetType
{
    using System;
    using MediatR;
    using Requests.WinLossByBettype;
    using Services;
    using ViewModels.WinLossByBettype;

    public class WinLossByBetTypeHandler :
        WinLossByBetTypeBaseHandler,
        IRequestHandler<WinLossByBettypeQuery, WinLossByBettypeViewModel>
    {
        private readonly ReportDateService reportDateService;

        public WinLossByBetTypeHandler(WinLossByBetTypeService reportService, ReportDateService reportDateService) : base(reportService)
        {
            this.reportDateService = reportDateService;
        }

        public WinLossByBettypeViewModel Handle(WinLossByBettypeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByBettypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByBetType(winLossByBettypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByBettypeViewModel(reportData, grandTotal, message, winLossByBettypeParam, reportDateService.GetBalanceUpDay());
        }
    }
}