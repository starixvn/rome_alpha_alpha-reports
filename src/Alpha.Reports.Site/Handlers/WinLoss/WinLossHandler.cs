namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
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
        private readonly string sortPropertyNameDefault = "CustName";

        public WinLossHandler(
            ProductService productService,
            WinLossService reportService) : base(productService, reportService)
        {
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

            var breadcrumbLevels = BuildBreadcrumbLevels(message);

            var sortPropertyName = GetSortedProperty(message.CustLevelId, message.SortPropertyName);
            var viewModel = new WinLossViewModel(reportData.ToList(), custLevel)
            {
                CustId = winLossParam.CustId,
                CustName = message.CustName,
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

                BreadcrumbLevelsJson = ToJson(breadcrumbLevels),
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                BreadcrumbLevels = breadcrumbLevels
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