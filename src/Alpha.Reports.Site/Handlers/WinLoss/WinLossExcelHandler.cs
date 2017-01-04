namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Linq;
    using App_GlobalResources;
    using Enumerations;
    using Extensions;
    using Fanex.Common;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Requests.WinLoss;
    using Services;
    using ViewModels;
    using ViewModels.WinLoss;
    using WinLoss;
    using Models = Reports.Models.Winloss;

    public class WinLossExcelHandler :
        WinLossBaseHandler,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<Models.WinLossModel> winlossWorkbook;

        public WinLossExcelHandler(
            ProductService productService,
            WinLossService reportService,
            Workbook<Models.WinLossModel> workbook) : base(productService, reportService)
        {
            winlossWorkbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var winLossParam = CreateWinLossParamater(message, userLevel);
            var reportData = reportService.GetWinLoss(winLossParam);
            var winlossViewModel = new WinLossViewModel(reportData.ToList())
            {
                UserLevel = userLevel
            };

            var products = message.UserSelectedProductIds.ToListInt();

            var reportTitle = $"{winlossViewModel.Title} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));

            var templatePath = GetExcelTemplatePath(message.ReportName, GetFileName(message.UserLevel));
            var templateInfo = new TemplateInfo(templatePath: templatePath, noInfomationText: Resources.NoInformationIsAvailable);

            var result = winlossWorkbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(Resources.ResourceManager.GetString)
                .Body(winlossViewModel.ReportData, GetWinLossRowMapper(message, isTotal: false))
                .Footer(winlossViewModel.GrantTotal, GetWinLossRowMapper(message, isTotal: true))
                .Sheet(winlossViewModel.Title)
                .ExportToBytes();

            return new ExcelFileViewModel(
                   $"{winlossViewModel.Title}-{message.FromDate.ToShortDateWithSlash()}-{message.ToDate.ToShortDateWithSlash()}",
                   result);
        }

        internal string GetFileName(UserLevel userLevel) =>
                    userLevel.IsSuperLevel()
                        ? "Master"
                        : userLevel.IsMasterLevel()
                            ? "Agent"
                            : "Member";

        private Mapper<Models.WinLossModel> GetWinLossRowMapper(WinLossExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<Models.WinLossModel>()
                .Map(Column.A, winLoss => isTotal ? Resources.Total : winLoss.CustName)
                .Map(Column.B, winLoss => winLoss.Turnover)
                .Map(Column.C, winLoss => winLoss.GrossComm);

            if (message.UserLevel.IsAgentLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.MemberNetWinLoss.WinLoss)
                 .Map(Column.E, winLoss => winLoss.MemberNetWinLoss.Comm)
                 .Map(Column.F, winLoss => winLoss.MemberNetWinLoss.Total)
                 .Map(Column.G, winLoss => winLoss.AgentNetWinLoss.WinLoss)
                 .Map(Column.H, winLoss => winLoss.AgentNetWinLoss.Comm)
                 .Map(Column.I, winLoss => winLoss.AgentNetWinLoss.Total)
                 .Map(Column.J, winLoss => winLoss.Company);
            }

            if (message.UserLevel.IsMasterLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.AgentNetWinLoss.Total)
                   .Map(Column.E, winLoss => winLoss.MasterNetWinLoss.WinLoss)
                   .Map(Column.F, winLoss => winLoss.MasterNetWinLoss.Comm)
                   .Map(Column.G, winLoss => winLoss.MasterNetWinLoss.Total)
                   .Map(Column.H, winLoss => winLoss.Company);
            }

            if (message.UserLevel.IsSuperLevel())
            {
                mapper.Map(Column.D, winLoss => winLoss.MasterNetWinLoss.Total)
                    .Map(Column.E, winLoss => winLoss.SuperNetWinLoss.WinLoss)
                    .Map(Column.F, winLoss => winLoss.SuperNetWinLoss.Comm)
                    .Map(Column.G, winLoss => winLoss.SuperNetWinLoss.Total)
                    .Map(Column.H, winLoss => winLoss.Company);
            }

            return mapper;
        }
    }
}