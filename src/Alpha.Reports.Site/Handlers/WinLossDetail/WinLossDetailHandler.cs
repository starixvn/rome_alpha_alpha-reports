namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Constants;
    using MediatR;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class WinLossDetailHandler :
        WinLossDetailBaseHandler,
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>
    {
        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService) : base(productService, reportService)
        {
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossDetailParam = CreateParameter(message, message.CustLevel);

            var reportData = reportService.GetWinLossDetail(winLossDetailParam);

            var canSort = reportData.DataSource != null && reportData.DataSource.Count() > 1;
            var viewModel = new WinLossDetailViewModel(BuildBreadcrumbLevels(message))
            {
                CustId = winLossDetailParam.CustId,
                CustName = message.CustName,
                CustLevel = message.CustLevel,
                FromDate = winLossDetailParam.FromDate,
                ToDate = winLossDetailParam.ToDate,
                Title = GetReportNameByLevel(message.CustLevel),

                Products = BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossDetailParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                ReportData = reportData,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,

                SortPropertyName = canSort ? winLossDetailParam.SortByPropertyName : DefaultVariables.SortPropertyName,
                IsDescendingSort = canSort && winLossDetailParam.IsDescendingSort,
                IsSetDefaultStackMode = message.IsSetDefaultStackMode
            };

            return viewModel;
        }

        private string GetReportNameByLevel(UserLevel userLevel)
            => userLevel.IsSuperLevel()
            ? Resources.MasterWinLossDetail
            : userLevel.IsMasterLevel()
                ? Resources.AgentWinLossDetail
                : Resources.MemberWinLossDetail;
    }
}