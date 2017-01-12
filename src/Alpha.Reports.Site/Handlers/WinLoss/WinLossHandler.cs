namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using Requests.WinLoss;
    using Services;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        WinLossBaseHandler,
        IRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly IUser user;
        private readonly string sortPropertyNameDefault = "CustName";

        public WinLossHandler(
            ProductService productService,
            WinLossService reportService,
            IUser user) : base(productService, reportService)
        {
            this.user = user;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public WinLossViewModel Handle(WinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var custLevel = Enumeration.FromValue<UserLevel>(message.CustLevelId);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossParam = CreateWinLossParamater(message, custLevel);
            var reportData = reportService.GetWinLoss(winLossParam);

            var drillDownInfo = BuildDrillDownInfo(message);

            var sortPropertyName = GetSortedProperty(message.CustLevelId, message.SortPropertyName);
            var viewModel = new WinLossViewModel(reportData.ToList())
            {
                CustId = winLossParam.CustId,
                CustName = message.CustName,
                CustLevel = custLevel,
                FromDate = winLossParam.FromDate,
                ToDate = winLossParam.ToDate,

                Products = BuildProductSelectListItems(winLossParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,
                IsDescendingSort = message.IsDescendingSort,
                SortPropertyName = sortPropertyName,

                DrillDownInfoJson = ToJson(drillDownInfo),
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    drillDownInfo,
                    message,
                    "WinLoss",
                    "Index",
                    (custInfo, query, drillDownInfoByLevelJson) =>
                    {
                        var routeValues = new WinLossQuery(user);
                        routeValues.CustId = custInfo.CustId;
                        routeValues.CustName = custInfo.CustName;
                        routeValues.FromDate = message.FromDate;
                        routeValues.ToDate = message.ToDate;

                        routeValues.UserSelectedProductIds = message.UserSelectedProductIds;
                        routeValues.SortPropertyName = sortPropertyName;
                        routeValues.IsDescendingSort = message.IsDescendingSort;
                        routeValues.IsStackMode = message.IsStackMode;
                        routeValues.IsHistoricReport = message.IsHistoricReport;
                        routeValues.IsFilterVisible = message.IsFilterVisible;
                        routeValues.UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
                        routeValues.CustLevelId = custInfo.CustLevelId;
                        routeValues.DrillDownInfoJson = drillDownInfoByLevelJson;

                        return routeValues;
                    })
            };

            return viewModel;
        }

        private string GetSortedProperty(byte userLevelId, string sortedProperty)
        {
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);

            var isInRange =
                userLevel.IsSuperLevel()
                || (userLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (userLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            if (!isInRange || string.IsNullOrEmpty(sortedProperty))
            {
                return sortPropertyNameDefault;
            }

            return sortedProperty;
        }
    }
}