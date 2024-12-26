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

    public class SettlementHandler :
        IAsyncRequestHandler<SettlementQuery, SettlementViewModel>,
        IAsyncRequestHandler<SettlementGetDataQuery, SettlementGetDataViewModel>
    {
        private readonly WhiteLabelReportService reportService;

        public SettlementHandler(WhiteLabelReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<SettlementViewModel> Handle(SettlementQuery message)
        {
            var whiteLabelsTask = message.ViewerLevel.IsNotAgentLevel()
                ? reportService.GetWhiteLabels(message.ViewerId, message.Site)
                : Task.FromResult(Enumerable.Empty<IdNameModel>());
            var settlementTask = GetSettlement(message);
            await Task.WhenAll(whiteLabelsTask, settlementTask);

            return new SettlementViewModel(message, await whiteLabelsTask, await settlementTask);
        }

        public async Task<SettlementGetDataViewModel> Handle(SettlementGetDataQuery message)
            => new SettlementGetDataViewModel(message.ViewerLevel, await GetSettlement(message));

        private Task<ReportData<SettlementModel>> GetSettlement(SettlementGetDataQuery message)
        {
            if (message.ViewerLevel.IsAgentLevel())
            {
                message.WhiteLabels = null;
                message.Username = null;
            }

            return reportService.GetSettlement(new ReportParam(
                message.ViewerId,
                message.FromDate.FirstDayOfMonth(),
                message.ToDate.LastDayOfMonth(),
                message.WhiteLabels,
                message.Username,
                message.Site));
        }
    }
}