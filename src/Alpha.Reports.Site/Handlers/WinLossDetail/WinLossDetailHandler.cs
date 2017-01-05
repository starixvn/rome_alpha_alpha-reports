namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using Services.WinLossDetail;
    using ViewModels.WinLossDetail;

    [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>
    {
        private readonly IUser user;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            IUser user) : base(productService, reportService)
        {
            this.user = user;
        }

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
            var drillDownInfo = BuildDrillDownInfo(message);

            var viewModel = new WinLossDetailViewModel
            {
                CustId = winLossDetailParam.CustId,
                CustName = message.CustName,
                UserLevel = userLevel,
                FromDate = winLossDetailParam.FromDate,
                ToDate = winLossDetailParam.ToDate,
                Title = GetReportNameByLevel(userLevel),

                Products = BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossDetailParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                ReportData = reportData,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,

                SortPropertyName = winLossDetailParam.SortByPropertyName,
                IsDescendingSort = winLossDetailParam.IsDescendingSort,
                HasSubmittedParams = message.HasSubmittedParams,
                DrillDownInfoJson = ToJson(drillDownInfo),
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    drillDownInfo,
                    message,
                    "WinLossDetail",
                    "Index",
                    (custInfo, query, drillDownInfoByLevelJson) =>
                    {
                        var routeValues = new WinLossDetailQuery(user);
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
                        routeValues.DrillDownInfoJson = drillDownInfoByLevelJson;

                        return routeValues;
                    })
            };

            return viewModel;
        }
    }
}