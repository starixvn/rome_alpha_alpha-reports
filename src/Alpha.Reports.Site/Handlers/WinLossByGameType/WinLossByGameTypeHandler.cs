namespace Alpha.Reports.Site.Handlers.WinLossByGameType
{
    using System;
    using MediatR;
    using Requests.WinLossByGameType;
    using Services;
    using ViewModels.WinLossByGameType;

    public class WinLossByGameTypeHandler :
         WinLossByGameTypeBaseHandler,
         IRequestHandler<WinLossByGameTypeQuery, WinLossByGameTypeViewModel>
    {
        public WinLossByGameTypeHandler(WinLossByGameTypeService reportService) : base(reportService)
        {
        }

        public WinLossByGameTypeViewModel Handle(WinLossByGameTypeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByGameTypeParam = CreateWinLossByGameTypeParamater(message);
            var reportData = reportService.GetWinLossByGameType(winLossByGameTypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new WinLossByGameTypeViewModel(reportData, grandTotal, message, winLossByGameTypeParam);
        }
    }
}