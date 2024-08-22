namespace Alpha.Reports.Site.Features.WhiteLabelReport
{
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.WhiteLabelReport;
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Features.WhiteLabelReport.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.WhiteLabelReport.Requests;
    using Alpha.Reports.Site.Handlers;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class DailyWinLossExcelHandler :
        BaseHandler,
        IAsyncRequestHandler<DailyWinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly WhiteLabelReportService reportService;
        private readonly Workbook<DailyWinLossModel> workbook;

        public DailyWinLossExcelHandler(
            WhiteLabelReportService reportService,
            Workbook<DailyWinLossModel> workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(DailyWinLossExcelQuery message)
        {
            var reportData = await GetDailyWinLoss(message);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message.ViewerLevel))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetRowMapper(message, false))
                .Footer(reportData.TotalData, GetRowMapper(message, true))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), result);
        }

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

        private TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                GetExcelTemplatePath($"DailyWinLoss{viewerLevel.GetDirectDownLineLevel().DisplayName}"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private static string GetReportTitle(DailyWinLossExcelQuery message)
            => $"{Resources.DailyWinLossReport} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}";

        internal static Mapper<DailyWinLossModel> GetRowMapper(DailyWinLossExcelQuery message, bool isTotal)
        {
            if (message.ViewerLevel.IsSuperLevel())
            {
                return GetSuperRowMapper(isTotal);
            }

            if (message.ViewerLevel.IsMasterLevel())
            {
                return GetMaterRowMapper(isTotal);
            }

            return GetAgentRowMapper(isTotal);
        }

        internal static Mapper<DailyWinLossModel> GetSuperRowMapper(bool isTotal)
        {
            var mapper = new Mapper<DailyWinLossModel>();
            if (isTotal)
            {
                mapper.Map(A, winloss => Resources.Total);
            }
            else
            {
                mapper
                    .Map(A, winloss => winloss.WinLostDate)
                    .Map(B, winloss => winloss.WhiteLabelName)
                    .Map(C, winloss => winloss.Agent)
                    .Map(D, winloss => winloss.Master);
            }

            return mapper
                .Map(E, winloss => winloss.MemberWinLoss)
                .Map(F, winloss => winloss.Promotion);
        }

        internal static Mapper<DailyWinLossModel> GetMaterRowMapper(bool isTotal)
        {
            var mapper = new Mapper<DailyWinLossModel>();
            if (isTotal)
            {
                mapper.Map(A, winloss => Resources.Total);
            }
            else
            {
                mapper
                    .Map(A, winloss => winloss.WinLostDate)
                    .Map(B, winloss => winloss.WhiteLabelName)
                    .Map(C, winloss => winloss.Agent);
            }

            return mapper
                .Map(D, winloss => winloss.MemberWinLoss)
                .Map(E, winloss => winloss.Promotion);
        }

        internal static Mapper<DailyWinLossModel> GetAgentRowMapper(bool isTotal)
        {
            var mapper = new Mapper<DailyWinLossModel>();
            if (isTotal)
            {
                mapper.Map(A, winloss => Resources.Total);
            }
            else
            {
                mapper.Map(A, winloss => winloss.WinLostDate);
            }

            return mapper
                .Map(B, winloss => winloss.MemberWinLoss)
                .Map(C, winloss => winloss.Promotion);
        }

        private string GetExcelFileName(DailyWinLossExcelQuery message)
           => string.Join(
               "-",
               Resources.DailyWinLossReport,
               message.CustName,
               message.FromDate.ToShortDateWithSlash(),
               message.ToDate.ToShortDateWithSlash());
    }
}