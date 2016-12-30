namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Enumerations;
    using MediatR;
    using Models;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetail;

    [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossDetailHandler :
        BaseWinLossDetailHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>
    {
        private readonly User user;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            User user) : base(productService, reportService)
        {
            this.user = user;
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var products = ProductService.GetProductsForCustomer(message.CustId);

            var winLossDetailParam = CreateParameter(message, userLevel);

            var reportData = ReportService.GetWinLossDetail(winLossDetailParam);

            var viewModel = new WinLossDetailViewModel
            {
                CustId = winLossDetailParam.CustId,
                CustName = message.CustName,
                UserLevel = userLevel,
                FromDate = winLossDetailParam.FromDate,
                ToDate = winLossDetailParam.ToDate,
                Title = GetReportTitle(userLevel),

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
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    message,
                    "WinLossDetail",
                    "Index",
                    (custInfo, query, breadcrumbLevels) =>
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
                        routeValues.BreadcrumbLevels = breadcrumbLevels;

                        return routeValues;
                    })
            };

            return viewModel;
        }
    }
}