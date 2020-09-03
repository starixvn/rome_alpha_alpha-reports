namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using Alpha.Web.Helpers;
    using MediatR;
    using Requests.WinLoss;
    using Services;
    using System;
    using ViewModels.WinLoss;

    public class EnteredSportResultHandler :
        BaseHandler,
        IRequestHandler<EnteredSportResultQuery, EnteredResultViewModel>
    {
        private readonly WinLossService reportService;
        private readonly BrowserHelper browserHelper;

        public EnteredSportResultHandler(WinLossService reportService, BrowserHelper browserHelper)
        {
            this.reportService = reportService;
            this.browserHelper = browserHelper;
        }

        public EnteredResultViewModel Handle(EnteredSportResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var reportData = reportService.GetEnteredSportResults();
            var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

            return new EnteredResultViewModel(message, reportData, isOnlyMobile);
        }
    }
}