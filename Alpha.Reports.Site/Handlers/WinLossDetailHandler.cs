namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Constants;
    using Enumerations;
    using Extensions;
    using Fanex.Common;
    using Helpers;
    using MediatR;
    using Models;
    using Models.WinlossDetails;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLossDetails;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetails;

    public class WinLossDetailHandler :
        BaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ExportWinLossDetailQuery, byte[]>
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;

        private readonly Workbook<WinLossDetail> workbook;
        private readonly IUserSession userSession;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            Workbook<WinLossDetail> workbook,
            IUserSession userSession)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.workbook = workbook;
            this.userSession = userSession;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossDetailParam = CreateParameter(message, userLevel);

            var reportData = reportService.GetWinLossDetail(winLossDetailParam);

            var canSort = reportData.DataSource != null && reportData.DataSource.Count() > 1;
            var viewModel = new WinLossDetailViewModel
            {
                CustId = winLossDetailParam.CustId,
                CustName = message.CustName,
                UserLevel = userLevel,
                FromDate = winLossDetailParam.FromDate,
                ToDate = winLossDetailParam.ToDate,

                Products = BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossDetailParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                ReportData = reportData,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                IsDirectReportViewAction = message.IsDirectReportViewAction,

                SortPropertyName = canSort ? winLossDetailParam.SortByPropertyName : DefaultVariables.SortPropertyName,
                IsDescendingSort = canSort && winLossDetailParam.IsDescendingSort,
                HasSubmittedParams = message.HasSubmittedParams,
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    message,
                    (custInfo, query, breadcrumbLevels) =>
                    {
                        var routeValues = new WinLossDetailQuery(userSession);
                        routeValues.CustId = custInfo.CustId;
                        routeValues.CustName = custInfo.CustName;
                        routeValues.PageSize = message.PageSize;
                        routeValues.FromDate = message.FromDate;
                        routeValues.ToDate = message.ToDate;

                        routeValues.UserSelectedProductIds = message.UserSelectedProductIds;
                        routeValues.SortPropertyName = message.SortPropertyName;
                        routeValues.IsDescendingSort = message.IsDescendingSort;
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

        public byte[] Handle(ExportWinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = string.IsNullOrWhiteSpace(message.UserSelectedProductIds)
                ? Enumerable.Empty<int>()
                : message.UserSelectedProductIds.Split(',').Select(s => Converter.ToInt(s));
            var reportTitle = $"{message.Title} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of2} " + string.Join(", ", productService.GetProductNames(products));
            var templateInfo = new TemplateInfo(templatePath: message.TemplatePath, noInfomationText: Resources.NoInformationIsAvailable)
                 .FreezePanel(4, Column.B);

            var result = workbook
                .UseTemplate(templateInfo)
                .Title(reportTitle)
                .Header(ExcelHelper.HeaderTitleMapping)
                .Body(message.ReportData.DataSource, GetRowMapper(message, isTotal: false))
                .Footer(message.ReportData.TotalData, GetRowMapper(message, isTotal: true))
                .Sheet(message.Title)
                .ExportToBytes();

            return result;
        }

        private WinLossDetailParam CreateParameter(
            WinLossDetailQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

            return new WinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 userLevel,
                 message.SortPropertyName,
                 message.IsDescendingSort,
                 message.IsHistoricReport);
        }

        private Mapper<WinLossDetail> GetRowMapper(ExportWinLossDetailQuery message, bool isTotal)
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

            if (message.UserLevel.IsAgentLevel())
            {
                mapper.Map(Column.L, winLossDetail => winLossDetail.Company);
            }

            if (message.UserLevel.IsMasterOrSuperLevel())
            {
                mapper.Map(Column.L, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(Column.M, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(Column.N, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (message.UserLevel.IsMasterLevel())
            {
                mapper.Map(Column.O, winLossDetail => winLossDetail.Company);
            }

            if (message.UserLevel.IsSuperLevel())
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