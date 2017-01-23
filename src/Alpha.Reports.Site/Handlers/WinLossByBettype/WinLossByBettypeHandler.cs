namespace Alpha.Reports.Site.Handlers.WinLossByBettype
{
    using System;
    using System.Linq;
    using MediatR;
    using Reports.Models.WinlossByBettype;
    using Requests.WinLossByBettype;
    using ViewModels.WinLossByBettype;

    public class WinLossByBettypeHandler : IRequestHandler<WinLossByBettypeQuery, WinLossByBettypeViewModel>
    {
        public WinLossByBettypeHandler()
        {
        }

        public WinLossByBettypeViewModel Handle(WinLossByBettypeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var reportData = Enumerable.Empty<WinlossByBettypeModel>();
            var grandTotal = new WinlossByBettypeModel();

            return new WinLossByBettypeViewModel(reportData, grandTotal, message);
        }
    }
}