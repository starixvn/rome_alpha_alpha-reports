namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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

            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossParam = CreateWinLossParamater(message);
            var reportData = reportService.GetWinLoss(winLossParam);

            var breadcrumbLevels = BuildBreadcrumbLevels(message);
            var sortPropertyName = GetSortedProperty(message.CustLevel, message.SortPropertyName);

            var viewModel = new WinLossViewModel(reportData.ToList(), message.CustLevel)
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

        private string GetSortedProperty(UserLevel custLevel, string sortedProperty)
        {
            var isSortedPropertyInRange =
                custLevel.IsSuperLevel()
                || (custLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (custLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            if (!isSortedPropertyInRange || string.IsNullOrEmpty(sortedProperty))
            {
                return sortPropertyNameDefault;
            }

            return sortedProperty;
        }
    }
}