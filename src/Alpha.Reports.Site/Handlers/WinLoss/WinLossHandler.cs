namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
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
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using ViewModels.WinLoss;
    using Models = Models.Winloss;

    public class WinLossHandler :
        BaseHandler,
        IRequestHandler<EnteredMatchResultQuery, EnteredResultViewModel>,
        IRequestHandler<EnteredMatchResultExcelQuery, ExcelFileViewModel>,
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly Workbook<Models.WinLoss> winlossWorkbook;
        private readonly Workbook<EnteredMatchResult> enteredSportResultWorkbook;
        private readonly IUserSession userSession;

        public WinLossHandler(
            ProductService productService,
            WinLossService reportService,
            Workbook<EnteredMatchResult> enteredSportResultWorkbook,
            Workbook<Models.WinLoss> workbook,
            IUserSession userSession)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.enteredSportResultWorkbook = enteredSportResultWorkbook;
            this.winlossWorkbook = workbook;
            this.userSession = userSession;
        }

        public EnteredResultViewModel Handle(EnteredMatchResultQuery message)
        {
            return BuildEnteredResultViewModel(message);
        }

        public ExcelFileViewModel Handle(EnteredMatchResultExcelQuery message)
        {
            var resources = new Dictionary<string, string>
            {
                ["Yesterday"] = DateTimeManager.Now.AddDays(-1).ToShortDateWithSlash(),
                ["Today"] = DateTimeManager.Now.ToShortDateWithSlash()
            };

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
               .Header(key => resources.ContainsKey(key) ? resources[key] : Resources.ResourceManager.GetString(key))
               .Body(enteredResultViewModel.ReportData, GetEnteredSportResultRowMapper())
               .Sheet(enteredResultViewModel.Title)
               .ExportToBytes();

            return new ExcelFileViewModel(
               ExcelHelper.StandardizeFileName(Resources.MatchResultEntered),
               result);
        }

        public WinLossViewModel Handle(WinLossQuery message)
        {
            return BuildWinLossViewModel(message);
        }

        public ExcelFileViewModel Handle(WinLossExcelQuery message)
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

        private EnteredResultViewModel BuildEnteredResultViewModel(EnteredMatchResultQuery message)
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
                FromDate = DateTimeManager.Now,
                ToDate = DateTimeManager.Now,
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

        private Mapper<EnteredMatchResult> GetEnteredSportResultRowMapper()
        {
            var mapper = new Mapper<EnteredMatchResult>()
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