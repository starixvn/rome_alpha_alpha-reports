namespace Alpha.Reports.Site.Handlers.WinLossByBettype
{
    using System;
    using MediatR;
    using Requests.WinLossByBettype;
    using Services;
    using ViewModels.WinLossByBettype;
    using WinLoss;

    public class WinLossByBettypeHandler :
         WinLossByBettypeBaseHandler
        , IRequestHandler<WinLossByBettypeQuery, WinLossByBettypeViewModel>
    {
        public WinLossByBettypeHandler(WinLossByBettypeService reportService) : base(reportService)
        {
        }

        public WinLossByBettypeViewModel Handle(WinLossByBettypeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByBettypeParam = CreateWinLossParamater(message);
            var reportData = reportService.GetWinLossByBettype(winLossByBettypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByBettypeViewModel(reportData, grandTotal, message, winLossByBettypeParam);
        }
    }
}