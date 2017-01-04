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
    using Services.WinLoss;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        WinLossBaseHandler,
        IRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly IUser user;

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
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    message,
                    "WinLoss",
                    "WinLoss",
                    (custInfo, query, breadcrumbLevels) =>
                    {
                        var routeValues = new WinLossQuery(user);
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
    }
}