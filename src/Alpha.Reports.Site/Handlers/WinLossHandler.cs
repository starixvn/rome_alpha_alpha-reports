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
    using Models.Winloss;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLoss;
    using Requests.WinLossDetails;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        BaseHandler,
        IRequestHandler<EnteredResultQuery, EnteredResultViewModel>,
        IRequestHandler<ExportEnteredResultQuery, ExcelFileViewModel>,
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<ExportWinLossQuery, ExcelFileViewModel>
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly Workbook<WinLoss> winlossWorkbook;
        private readonly Workbook<EnteredSportResult> enteredSportResultWorkbook;
        private readonly IUserSession userSession;

        public WinLossHandler(
            ProductService productService,
            WinLossService reportService,
            Workbook<EnteredSportResult> enteredSportResultWorkbook,
            Workbook<WinLoss> workbook,
            IUserSession userSession)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.enteredSportResultWorkbook = enteredSportResultWorkbook;
            this.winlossWorkbook = workbook;
            this.userSession = userSession;
        }

        public EnteredResultViewModel Handle(EnteredResultQuery message)
        {
            return BuildEnteredResultViewModel(message);
        }

        public ExcelFileViewModel Handle(ExportEnteredResultQuery message)
        {
            var enteredResultViewModel = BuildEnteredResultViewModel(message);

            var templateInfo = new TemplateInfo(
                templatePath: message.TemplatePath,
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 0,
                headerStartIndex: 1,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

            var result = enteredSportResultWorkbook
               .UseTemplate(templateInfo)
               .Header(ExcelHelper.HeaderTitleMapping)
               .Body(enteredResultViewModel.ReportData, GetEnteredSportResultRowMapper())
               .Sheet(enteredResultViewModel.Title)
               .ExportToBytes();

            return new ExcelFileViewModel(
               ExcelHelper.StandardizeFileName(enteredResultViewModel.Title),
               result);
        }

        public WinLossViewModel Handle(WinLossQuery message)
        {
            return BuildWinLossViewModel(message);
        }

        public ExcelFileViewModel Handle(ExportWinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var winlossViewModel = BuildWinLossViewModel(message);

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
               ExcelHelper.StandardizeFileName(
                   $"{winlossViewModel.Title}-{winlossViewModel.FromDate.ToShortDateWithSlash()}-{winlossViewModel.ToDate.ToShortDateWithSlash()}"),
               result);
        }

        private EnteredResultViewModel BuildEnteredResultViewModel(EnteredResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            var reportData = reportService.GetEnteredSportResults();

            var viewModel = new EnteredResultViewModel
            {
                UserLevel = userLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                Language = message.Language,
                ReportData = reportData,
                UserSelectedDateRangeOption = 0
            };

            return viewModel;
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

            var viewModel = new WinLossViewModel(reportData.ToList())
            {
                CustId = winLossParam.CustId,
                CustName = message.CustName,
                UserLevel = userLevel,
                FromDate = winLossParam.FromDate,
                ToDate = winLossParam.ToDate,

                Products = BuildProductSelectListItems(winLossParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                IsDirectReportViewAction = message.IsDirectReportViewAction,
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    message,
                    "WinLoss",
                    "WinLoss",
                    (custInfo, query, breadcrumbLevels) =>
                    {
                        var routeValues = new WinLossQuery(userSession);
                        routeValues.CustId = custInfo.CustId;
                        routeValues.CustName = custInfo.CustName;
                        routeValues.FromDate = message.FromDate;
                        routeValues.ToDate = message.ToDate;

                        routeValues.UserSelectedProductIds = message.UserSelectedProductIds;
                        routeValues.IsStackMode = message.IsStackMode;
                        routeValues.IsHistoricReport = message.IsHistoricReport;
                        routeValues.IsFilterVisible = message.IsFilterVisible;
                        routeValues.UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
                        routeValues.UserLevelId = custInfo.UserLevelId;
                        routeValues.BreadcrumbLevels = breadcrumbLevels;

                        return routeValues;
                    })
            };

            return viewModel;
        }

        private Mapper<WinLoss> GetWinLossRowMapper(ExportWinLossQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLoss>()
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

        private WinLossParam CreateWinLossParamater(
            WinLossQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

            return new WinLossParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 userLevel,
                 message.IsHistoricReport);
        }
    }
}