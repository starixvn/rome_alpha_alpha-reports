namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using AppResources;
    using Extensions;
    using Helpers;
    using Models.WinlossDetails;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Services;

    public class WinLossDetailExcelExporterImpl : WinLossDetailExcelExporter
    {
        private ProductService productService;

        private Workbook<WinLossDetail> workbook;

        private ExportWinLossDetailParam exportWinLossDetailParam;

        public WinLossDetailExcelExporterImpl(ProductService productService, Workbook<WinLossDetail> workbook)
        {
            this.productService = productService;
            this.workbook = workbook;
        }

        public byte[] Export(ExportExcelParam<WinLossDetail> param, ExportWinLossDetailParam reportParam)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (reportParam == null)
            {
                throw new ArgumentNullException(nameof(reportParam));
            }

            exportWinLossDetailParam = reportParam;

            var reportTitle = $"{param.ReportName} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(reportParam.UserSelectedProductIds));

            var templateInfo = new TemplateInfo(templatePath: param.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable)
                 .FreezePanel(4, Column.B);

            var result = workbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(param.ReportData.DataSource, GetRowMapper(isTotal: false))
                .Footer(param.ReportData.TotalData, GetRowMapper(isTotal: true))
                .Sheet(param.SheetName)
                .ExportToBytes();

            return result;
        }

        private Mapper<WinLossDetail> GetRowMapper(bool isTotal)
        {
            var mapper = new Mapper<WinLossDetail>()
                .Map(Column.A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(Column.B, winLossDetail => winLossDetail.NumberOfTickets)
                .Map(Column.C, winLossDetail => winLossDetail.Turnover)
                .Map(Column.D, winLossDetail => winLossDetail.NetTurnover)
                .Map(Column.E, winLossDetail => winLossDetail.GrossComm)
                .Map(Column.F, winLossDetail => winLossDetail.MemberNetWinLoss.WinLoss)
                .Map(Column.G, winLossDetail => winLossDetail.MemberNetWinLoss.Comm)
                .Map(Column.H, winLossDetail => winLossDetail.MemberNetWinLoss.Total)
                .Map(Column.I, winLossDetail => winLossDetail.AgentNetWinLoss.WinLoss)
                .Map(Column.J, winLossDetail => winLossDetail.AgentNetWinLoss.Comm)
                .Map(Column.K, winLossDetail => winLossDetail.AgentNetWinLoss.Total);

            if (exportWinLossDetailParam.UserLevel.IsAgentLevel())
            {
                mapper.Map(Column.L, winLossDetail => winLossDetail.Company);
            }

            if (exportWinLossDetailParam.UserLevel.IsMasterOrSuperLevel())
            {
                mapper.Map(Column.L, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(Column.M, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(Column.N, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (exportWinLossDetailParam.UserLevel.IsMasterLevel())
            {
                mapper.Map(Column.O, winLossDetail => winLossDetail.Company);
            }

            if (exportWinLossDetailParam.UserLevel.IsSuperLevel())
            {
                mapper.Map(Column.O, winLossDetail => winLossDetail.SuperNetWinLoss.WinLoss)
                    .Map(Column.P, winLossDetail => winLossDetail.SuperNetWinLoss.Comm)
                    .Map(Column.Q, winLossDetail => winLossDetail.SuperNetWinLoss.Total)
                    .Map(Column.R, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }
    }
}
