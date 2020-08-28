namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using Alpha.Reports.Constants;
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
                .Map(Column.B, winLoss => winLoss.Turnover)
                .Map(Column.C, winLoss => winLoss.GrossComm);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.MemberNetWinLoss.WinLoss)
                 .Map(Column.E, winLoss => winLoss.MemberNetWinLoss.Comm)
                 .Map(Column.F, winLoss => winLoss.MemberNetWinLoss.Total)
                 .Map(Column.G, winLoss => winLoss.AgentNetWinLoss.WinLoss)
                 .Map(Column.H, winLoss => winLoss.AgentNetWinLoss.Comm)
                 .Map(Column.I, winLoss => winLoss.AgentNetWinLoss.Total)
                 .Map(Column.J, winLoss => winLoss.Company);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.AgentNetWinLoss.Total)
                   .Map(Column.E, winLoss => winLoss.MasterNetWinLoss.WinLoss)
                   .Map(Column.F, winLoss => winLoss.MasterNetWinLoss.Comm)
                   .Map(Column.G, winLoss => winLoss.MasterNetWinLoss.Total)
                   .Map(Column.H, winLoss => winLoss.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.MasterNetWinLoss.Total)
                    .Map(Column.E, winLoss => winLoss.SuperNetWinLoss.WinLoss)
                    .Map(Column.F, winLoss => winLoss.SuperNetWinLoss.Comm)
                    .Map(Column.G, winLoss => winLoss.SuperNetWinLoss.Total)
                    .Map(Column.H, winLoss => winLoss.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable);

        private string GetReportTitle(WinLossParam param)
        {
            var sabaclubName = productDetailService.GetSabaclubNameExportExcel(param.UserSelectedResourceIds);
            var productNames = productService.ConcatProductNames(param.UserSelectedProductIds, ProductIdConstants.SabaClub, sabaclubName);

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