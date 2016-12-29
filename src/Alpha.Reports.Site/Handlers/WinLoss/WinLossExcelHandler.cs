namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Enumerations;
    using Extensions;
    using Fanex.Common;
    using Helpers;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using Requests.WinLoss;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using ViewModels.WinLoss;
    using WinLoss;
    using Models = Models.Winloss;

    public class WinLossExcelHandler :
        BaseWinLossHandler,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly Workbook<Models.WinLoss> winlossWorkbook;
        private readonly IUserSession userSession;

        public WinLossExcelHandler(
            ProductService productService,
            WinLossService reportService,
            Workbook<Models.WinLoss> workbook,
            IUserSession userSession) : base(productService, reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.winlossWorkbook = workbook;
            this.userSession = userSession;
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
            var winlossViewModel = new WinLossViewModel(reportData.ToList());

            var products = string.IsNullOrWhiteSpace(message.UserSelectedProductIds)
                ? Enumerable.Empty<int>()
                : message.UserSelectedProductIds.Split(',').Select(s => Converter.ToInt(s));

            var reportTitle = $"{winlossViewModel.Title} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));

            var templateInfo = new TemplateInfo(templatePath: message.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable);

            var result = winlossWorkbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(winlossViewModel.ReportData, GetWinLossRowMapper(message, isTotal: false))
                .Footer(winlossViewModel.GrantTotal, GetWinLossRowMapper(message, isTotal: true))
                .Sheet(winlossViewModel.Title)
                .ExportToBytes();

            return new ExcelFileViewModel(
                   $"{winlossViewModel.Title}-{winlossViewModel.FromDate.ToShortDateWithSlash()}-{winlossViewModel.ToDate.ToShortDateWithSlash()}",
                   result);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private WinLossViewModel BuildWinLossViewModel(WinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossParam = CreateWinLossParamater(message, userLevel);
            var reportData = reportService.GetWinLoss(winLossParam);

            var viewModel = new WinLossViewModel(reportData.ToList());
         
            return viewModel;
        }

        private Mapper<Models.WinLoss> GetWinLossRowMapper(WinLossExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<Models.WinLoss>()
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