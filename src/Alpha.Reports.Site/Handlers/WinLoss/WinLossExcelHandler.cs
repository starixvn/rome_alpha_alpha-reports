namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Services;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossExcelHandler :
        WinLossBaseHandler,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<WinLossModel> winlossWorkbook;

        public WinLossExcelHandler(
            ProductService productService,
            WinLossService reportService,
            Workbook<WinLossModel> workbook) : base(productService, reportService)
        {
            winlossWorkbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossParam = CreateWinLossParamater(message);
            var reportData = reportService.GetWinLoss(winLossParam);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winlossViewModel = new WinLossViewModel(
                                            reportData, 
                                            message,
                                            winLossParam,
                                            Enumerable.Empty<CustInfo>(),
                                            BuildProductSelectListItems(winLossParam.UserSelectedProductIds, products));

            var reportTitle = BuildReportTitlte(winlossViewModel.Title, message);
            var templatePath = GetExcelTemplatePath(GetFileName(message.CustLevel));
            var templateInfo = new TemplateInfo(templatePath: templatePath, noInfomationText: Resources.NoInformationIsAvailable);

            var result = winlossWorkbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(Resources.ResourceManager.GetString)
                .Body(winlossViewModel.ReportData, GetWinLossRowMapper(message, isTotal: false))
                .Footer(winlossViewModel.GrantTotal, GetWinLossRowMapper(message, isTotal: true))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            var excelFileName = BuildExcelFileName(message);
            return new ExcelFileViewModel(excelFileName, result);
        }

        internal string GetFileName(UserLevel userLevel) =>
                    userLevel.IsSuperLevel()
                        ? "Master"
                        : userLevel.IsMasterLevel()
                            ? "Agent"
                            : "Member";

        private Mapper<WinLossModel> GetWinLossRowMapper(WinLossExcelQuery message, bool isTotal)
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

        private string BuildReportTitlte(string title, WinLossExcelQuery message)
        {
            var products = message.UserSelectedProductIds.ToListInt();

            return $"{title} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));
        }

        private string BuildExcelFileName(WinLossExcelQuery message)
        {
          return $"{Resources.WinLossTitle}-{message.CustName}-{message.FromDate.ToShortDateWithSlash()}-{message.ToDate.ToShortDateWithSlash()}";
        }
    }
}