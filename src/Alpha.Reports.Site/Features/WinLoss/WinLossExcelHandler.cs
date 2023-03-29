namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Services;
    using ViewModels;

    public class WinLossExcelHandler :
        WinLossBaseHandler,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossModel> workbook;

        public WinLossExcelHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService reportService,
            Workbook<WinLossModel> workbook) : base(productService, productDetailService, reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossParam = CreateWinLossParamater(message);
            var reportData = reportService.GetWinLoss(winLossParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Title(GetReportTitle(winLossParam))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData, GetBodyRowMapper(message))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossParam), result);
        }

        private Mapper<WinLossModel> GetBodyRowMapper(WinLossExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossModel> GetTotalRowMapper(WinLossExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<WinLossModel> GetRowMapper(WinLossExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossModel>()
                .Map(Column.A, winLoss => isTotal ? Resources.Total : winLoss.CustName)
                .Map(Column.B, winLoss => isTotal ? string.Empty : GetFirstLastName(winLoss.FirstName, winLoss.LastName))
                .Map(Column.C, winLoss => winLoss.Turnover)
                .Map(Column.D, winLoss => winLoss.GrossComm);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(Column.E, winLoss => winLoss.MemberNetWinLoss.WinLoss)
                 .Map(Column.F, winLoss => winLoss.MemberNetWinLoss.Comm)
                 .Map(Column.G, winLoss => winLoss.MemberNetWinLoss.Total)
                 .Map(Column.H, winLoss => winLoss.AgentNetWinLoss.WinLoss)
                 .Map(Column.I, winLoss => winLoss.AgentNetWinLoss.Comm)
                 .Map(Column.J, winLoss => winLoss.AgentNetWinLoss.Total)
                 .Map(Column.K, winLoss => winLoss.Company);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(Column.E, winLoss => winLoss.AgentNetWinLoss.Total)
                   .Map(Column.F, winLoss => winLoss.MasterNetWinLoss.WinLoss)
                   .Map(Column.G, winLoss => winLoss.MasterNetWinLoss.Comm)
                   .Map(Column.H, winLoss => winLoss.MasterNetWinLoss.Total)
                   .Map(Column.I, winLoss => winLoss.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper.Map(Column.E, winLoss => winLoss.MasterNetWinLoss.Total)
                    .Map(Column.F, winLoss => winLoss.SuperNetWinLoss.WinLoss)
                    .Map(Column.G, winLoss => winLoss.SuperNetWinLoss.Comm)
                    .Map(Column.H, winLoss => winLoss.SuperNetWinLoss.Total)
                    .Map(Column.I, winLoss => winLoss.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReportTitle(WinLossParam param)
        {
            var replacedProductNames = productDetailService.GetProductNamesExportExcel(param.UserSelectedResourceIds);
            var productNames = productService.ConcatProductNames(param.UserSelectedProductIds, replacedProductNames);

            return $"{Resources.WinLossTitle} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()} {Resources.of2} {productNames}";
        }

        private string GetExcelFileName(WinLossExcelQuery message, WinLossParam winLossParam)
           => string.Join(
               "-",
               Resources.WinLossTitle,
               message.CustName,
               winLossParam.FromDate.ToShortDateWithSlash(),
               winLossParam.ToDate.ToShortDateWithSlash());
    }
}