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
    using Newtonsoft.Json;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLoss;
    using Requests.WinLossDetails;
    using Services;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        BaseHandler,
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<EnterResultQuery, EnteredResultViewModel>,
        IRequestHandler<ExportWinLossQuery, byte[]>,
        IRequestHandler<ExportEnteredResultQuery, byte[]>
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly Workbook<WinLoss> winlossWorkbook;
        private readonly Workbook<EnteredSportResult> enteredSportResultWorkbook;

        public WinLossHandler(
            ProductService productService,
            WinLossService reportService,
            Workbook<WinLoss> workbook,
            Workbook<EnteredSportResult> enteredSportResultWorkbook)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.winlossWorkbook = workbook;
            this.enteredSportResultWorkbook = enteredSportResultWorkbook;
        }

        public EnteredResultViewModel Handle(EnterResultQuery message)
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

        public WinLossViewModel Handle(WinLossQuery message)
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
                BreadcrumbLevels = BuildBreadcrumbLevels(message)
            };

            return viewModel;
        }

        public byte[] Handle(ExportWinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = string.IsNullOrWhiteSpace(message.UserSelectedProductIds)
                ? Enumerable.Empty<int>()
                : message.UserSelectedProductIds.Split(',').Select(s => Converter.ToInt(s));

            var reportTitle = $"{message.Title} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));

            var templateInfo = new TemplateInfo(templatePath: message.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable);

            var result = winlossWorkbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(message.ReportData, GetWinLossRowMapper(message, isTotal: false))
                .Footer(message.GrantTotal, GetWinLossRowMapper(message, isTotal: true))
                .Sheet(message.Title)
                .ExportToBytes();

            return result;
        }

        public byte[] Handle(ExportEnteredResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var reportTitle = message.Title;
            var templateInfo = new TemplateInfo(
                templatePath: message.TemplatePath, 
                noInfomationText: Resources.NoInformationIsAvailable, 
                titleStartIndex: 0, 
                headerStartIndex: 1, 
                bodyStartIndex: 3, 
                bodyEndIndex: 4);

            var result = enteredSportResultWorkbook
               .UseTemplate(templateInfo)
               .Title(reportTitle)
               .Header(ExcelHelper.HeaderTitleMapping)
               .Body(message.ReportData, GetEnteredSportResultRowMapper())
               .Sheet(message.Title)
               .ExportToBytes();

            return result;
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

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IEnumerable<ActionLinkViewModel> BuildBreadcrumbLevels(WinLossQuery message)
        {
            var drilldownLevels = new List<ActionLinkViewModel>();
            var breadcrumbLevels = new List<CustInfo>();

            if (!string.IsNullOrWhiteSpace(message.BreadcrumbLevels))
            {
                var messageBreadcrumbLevels = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(message.BreadcrumbLevels.Replace("'", "\""));

                foreach (var custInfo in messageBreadcrumbLevels)
                {
                    if (custInfo.UserLevelId == message.UserLevelId)
                    {
                        break;
                    }

                    breadcrumbLevels.Add(custInfo);

                    var actionLinkViewModel = new ActionLinkViewModel(
                        controllerName: "WinLoss",
                        actionName: "WinLoss",
                        linkText: custInfo.CustName,
                        routeValues: new WinLossQuery
                        {
                            CustId = custInfo.CustId,
                            CustName = custInfo.CustName,
                            FromDate = message.FromDate,
                            ToDate = message.ToDate,

                            UserSelectedProductIds = message.UserSelectedProductIds,
                            IsStackMode = message.IsStackMode,
                            IsHistoricReport = message.IsHistoricReport,
                            IsFilterVisible = message.IsFilterVisible,
                            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                            UserLevelId = custInfo.UserLevelId,
                            BreadcrumbLevels = JsonConvert.SerializeObject(breadcrumbLevels).Replace("\"", "'")
                        });

                    drilldownLevels.Add(actionLinkViewModel);
                }
            }

            breadcrumbLevels.Add(new CustInfo(message.CustId, message.CustName, message.UserLevelId));
            drilldownLevels.Add(new ActionLinkViewModel(
                controllerName: "WinLoss",
                actionName: "WinLoss",
                linkText: message.CustName,
                routeValues: new WinLossQuery
                {
                    CustId = message.CustId,
                    CustName = message.CustName,
                    FromDate = message.FromDate,
                    ToDate = message.ToDate,
                    UserSelectedProductIds = message.UserSelectedProductIds,

                    IsStackMode = message.IsStackMode,
                    IsHistoricReport = message.IsHistoricReport,
                    IsFilterVisible = message.IsFilterVisible,
                    UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                    UserLevelId = message.UserLevelId,
                    BreadcrumbLevels = JsonConvert.SerializeObject(breadcrumbLevels).Replace("\"", "'")
                }));

            return drilldownLevels;
        }
    }
}