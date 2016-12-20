namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Linq;
    using AppResources;
    using Extensions;
    using Helpers;
    using Models.Winloss;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Services;

    public class WinLossExcelExporterImpl : WinLossExcelExporter
    {
        private ProductService productService;

        private Workbook<WinLoss> winLossWorkbook;

        private Workbook<EnteredSportResult> enteredSportResultWorkbook;

        private ExportWinLossParam exportWinLossParam;

        public WinLossExcelExporterImpl(
            ProductService productService, 
            Workbook<WinLoss> workbook,
            Workbook<EnteredSportResult> enteredSportResultWorkbook)
        {
            this.productService = productService;
            this.winLossWorkbook = workbook;
            this.enteredSportResultWorkbook = enteredSportResultWorkbook;
        }

        public byte[] ExportWinLossExcel(ExportWinLossParam reportParam)
        {
            if (reportParam == null)
            {
                throw new ArgumentNullException(nameof(reportParam));
            }

            exportWinLossParam = reportParam;

            var reportTitle = $"{reportParam.ReportName} {reportParam.FromDate.ToShortDateWithSlash()} --> {reportParam.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(reportParam.UserSelectedProductIds));

            var templateInfo = new TemplateInfo(templatePath: reportParam.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable);

            var result = winLossWorkbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(reportParam.ReportData, GetWinLossRowMapper(isTotal: false))
                .Footer(reportParam.GrantTotal, GetWinLossRowMapper(isTotal: true))
                .Sheet(reportParam.SheetName)
                .ExportToBytes();

            return result;
        }

        public byte[] ExportEnteredResultExcel(ExportEnteredResultParam reportParam)
        {
            if (reportParam == null)
            {
                throw new ArgumentNullException(nameof(reportParam));
            }


            var reportTitle = reportParam.ReportName;

            var templateInfo = new TemplateInfo(templatePath: reportParam.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable, titleStartIndex: 0, headerStartIndex: 1, bodyStartIndex: 3, bodyEndIndex: 4);

            var result = enteredSportResultWorkbook
                .UseTemplate(templateInfo)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(reportParam.ReportData, GetEnteredSportResultRowMapper())
                .Sheet(reportParam.SheetName)
                .ExportToBytes();

            return result;
        }

        private Mapper<WinLoss> GetWinLossRowMapper(bool isTotal)
        {
            var mapper = new Mapper<WinLoss>()
                .Map(Column.A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(Column.B, winLossDetail => winLossDetail.Turnover)
                .Map(Column.C, winLossDetail => winLossDetail.GrossComm);

            if (exportWinLossParam.UserLevel.IsAgentLevel())
            {
                mapper.Map(Column.D, winLossDetail => winLossDetail.MemberNetWinLoss.WinLoss)
                 .Map(Column.E, winLossDetail => winLossDetail.MemberNetWinLoss.Comm)
                 .Map(Column.F, winLossDetail => winLossDetail.MemberNetWinLoss.Total)
                 .Map(Column.G, winLossDetail => winLossDetail.AgentNetWinLoss.WinLoss)
                 .Map(Column.H, winLossDetail => winLossDetail.AgentNetWinLoss.Comm)
                 .Map(Column.I, winLossDetail => winLossDetail.AgentNetWinLoss.Total)
                 .Map(Column.J, winLossDetail => winLossDetail.Company);
            }

            if (exportWinLossParam.UserLevel.IsMasterLevel())
            {
                mapper.Map(Column.D, winLossDetail => winLossDetail.AgentNetWinLoss.Total)
                   .Map(Column.E, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                   .Map(Column.F, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                   .Map(Column.G, winLossDetail => winLossDetail.MasterNetWinLoss.Total)
                   .Map(Column.H, winLossDetail => winLossDetail.Company);
            }

            if (exportWinLossParam.UserLevel.IsSuperLevel())
            {
                mapper.Map(Column.D, winLossDetail => winLossDetail.MasterNetWinLoss.Total)
                    .Map(Column.E, winLossDetail => winLossDetail.SuperNetWinLoss.WinLoss)
                    .Map(Column.F, winLossDetail => winLossDetail.SuperNetWinLoss.Comm)
                    .Map(Column.G, winLossDetail => winLossDetail.SuperNetWinLoss.Total)
                    .Map(Column.H, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }

        private Mapper<EnteredSportResult> GetEnteredSportResultRowMapper()
        {
            var mapper = new Mapper<EnteredSportResult>()
                .Map(Column.A, enteredSportResult => enteredSportResult.SportName)
                .Map(Column.B, enteredSportResult => enteredSportResult.EnterSportYesterday?.Entered ?? 0)
                .Map(Column.C, enteredSportResult => enteredSportResult.EnterSportYesterday?.NonEntered ?? 0)
                .Map(Column.D, enteredSportResult => enteredSportResult.EnterSportToday?.Entered ?? 0)
                .Map(Column.E, enteredSportResult => enteredSportResult.EnterSportToday?.NonEntered ?? 0);

            return mapper;
        }
    }
}
