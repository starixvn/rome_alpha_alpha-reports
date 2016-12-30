namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using Enumerations;
    using MediatR;
    using Reports.Models;
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
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            var reportData = reportService.GetEnteredSportResults();

            var viewModel = new EnteredResultViewModel
            {
                UserLevel = userLevel,
                FromDate = DateTimeManager.Now,
                ToDate = DateTimeManager.Now,
                Language = message.Language,
                ReportData = reportData,
                UserSelectedDateRangeOption = 0
            };

            return viewModel;
        }
    }
}