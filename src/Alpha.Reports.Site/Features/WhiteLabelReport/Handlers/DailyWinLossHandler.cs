namespace Alpha.Reports.Site.Features.WhiteLabelReport
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.WhiteLabelReport;
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Features.WhiteLabelReport.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WhiteLabelReport.Requests;
    using Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels;
    using MediatR;

    public class DailyWinLossHandler :
        IAsyncRequestHandler<DailyWinLossQuery, DailyWinLossViewModel>,
        IAsyncRequestHandler<DailyWinLossGetDataQuery, DailyWinLossGetDataViewModel>
    {
        private readonly WhiteLabelReportService reportService;

        public DailyWinLossHandler(WhiteLabelReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<DailyWinLossViewModel> Handle(DailyWinLossQuery message)
        {
            var whiteLabelsTask = message.ViewerLevel.IsNotAgentLevel()
                ? reportService.GetWhiteLabels(message.ViewerId, message.Site)
                : Task.FromResult(Enumerable.Empty<IdNameModel>());
            var winlossTask = GetDailyWinLoss(message);
            await Task.WhenAll(whiteLabelsTask, winlossTask);

            return new DailyWinLossViewModel(message, await whiteLabelsTask, await winlossTask);
        }

        public async Task<DailyWinLossGetDataViewModel> Handle(DailyWinLossGetDataQuery message)
            => new DailyWinLossGetDataViewModel(message.ViewerLevel, await GetDailyWinLoss(message));

        private Task<ReportData<DailyWinLossModel>> GetDailyWinLoss(DailyWinLossGetDataQuery message)
        {
            if (message.ViewerLevel.IsAgentLevel())
            {
                message.WhiteLabels = null;
                message.Username = null;
            }

            return reportService.GetDailyWinLoss(new ReportParam(
                message.ViewerId,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.WhiteLabels,
                message.Username,
                message.Site));
        }
    }
}