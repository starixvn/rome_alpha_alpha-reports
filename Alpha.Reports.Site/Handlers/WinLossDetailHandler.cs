namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Enumerations;
    using Excel.Params;
    using Excel.Services;
    using Extensions;
    using MediatR;
    using Models;
    using Models.WinlossDetails;
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
        private readonly WinLossDetailExcelExporter excelExporter;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            WinLossDetailExcelExporter excelExporter)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.excelExporter = excelExporter;
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

            var fileContents = excelExporter.Export(
                new ExportExcelParam<WinLossDetail>
                {
                    TemplatePath = message.TemplatePath,
                    ReportName = message.Title,
                    SheetName = message.Title,
                    ReportData = message.ReportData,
                    FromDate = message.FromDate,
                    ToDate = message.ToDate
                },
                new ExportWinLossDetailParam(message.UserLevel, message.UserSelectedProductIds));

            return fileContents;
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
    }
}