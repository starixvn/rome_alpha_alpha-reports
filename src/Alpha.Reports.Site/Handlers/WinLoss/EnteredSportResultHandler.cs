namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using MediatR;
    using Requests.WinLoss;
    using Services;
    using ViewModels.WinLoss;

    public class EnteredSportResultHandler :
        BaseHandler,
        IRequestHandler<EnteredSportResultQuery, EnteredResultViewModel>
    {
        private readonly WinLossService reportService;

        public EnteredSportResultHandler(WinLossService reportService)
        {
            this.reportService = reportService;
        }

        public EnteredResultViewModel Handle(EnteredSportResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var reportData = reportService.GetEnteredSportResults();

            return new EnteredResultViewModel(message, reportData);
        }
    }
}