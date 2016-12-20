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
    using Models.WinlossDetails;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLossDetails;
    using Services;
    using ViewModels.WinLossDetails;

    public class WinLossDetailHandler :
        BaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ExportWinLossDetailQuery, byte[]>
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;

        private Workbook<WinLossDetail> workbook;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            Workbook<WinLossDetail> workbook)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.workbook = workbook;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossDetailParam = CreateParamater(message, userLevel);

            var reportData = reportService.GetWinLossDetail(winLossDetailParam);

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

                SortPropertyName = winLossDetailParam.SortByPropertyName,
                IsDescendingSort = winLossDetailParam.IsDescendingSort,
                HasSubmittedParams = message.HasSubmittedParams,
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    message,
                    (custInfo, query, breadcrumbLevels) => new WinLossDetailQuery
                    {
                        CustId = custInfo.CustId,
                        CustName = custInfo.CustName,
                        PageSize = message.PageSize,
                        FromDate = message.FromDate,
                        ToDate = message.ToDate,

                        UserSelectedProductIds = message.UserSelectedProductIds,
                        SortPropertyName = message.SortPropertyName,
                        IsDescendingSort = message.IsDescendingSort,
                        IsStackMode = message.IsStackMode,
                        IsHistoricReport = message.IsHistoricReport,
                        IsFilterVisible = message.IsFilterVisible,
                        UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                        UserLevelId = custInfo.UserLevelId,
                        BreadcrumbLevels = breadcrumbLevels
                    })
            };

            return viewModel;
        }

        public byte[] Handle(ExportWinLossDetailQuery message)
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

        private WinLossDetailParam CreateParamater(
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