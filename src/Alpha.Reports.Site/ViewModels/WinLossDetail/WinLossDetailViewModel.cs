namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Requests.WinLossDetail;

    public class WinLossDetailViewModel : WinLossReportBaseViewModel<WinLossDetailModel>
    {
        public WinLossDetailViewModel(
            WinLossDetailQuery message,
            WinLossDetailParam param,
            PagedReportData<WinLossDetailModel> reportData,
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<SelectListItem> products)
            : base(message, param, breadcrumbLevel, products)
        {
            Title = GetReportNameByLevel(message.CustLevel);
            PageIndex = message.PageIndex;
            PageSize = message.PageSize;
            ReportData = reportData;

            var canSort = reportData.DataSource != null && reportData.DataSource.Count() > 1;
            SortPropertyName = canSort ? GetSortedProperty(message.CustLevel, message.SortPropertyName) : DefaultVariables.SortPropertyName;
            IsDescendingSort = canSort && param.IsDescendingSort;
        }

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(ReportData.CurrentPageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public string Title { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public PagedReportData<WinLossDetailModel> ReportData { get; }

        private string GetReportNameByLevel(UserLevel userLevel)
            => userLevel.IsSuperLevel()
            ? Resources.MasterWinLossDetail
            : userLevel.IsMasterLevel()
                ? Resources.AgentWinLossDetail
                : Resources.MemberWinLossDetail;

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(WinLossDetailModel model)
            => new Dictionary<string, object>(base.GetDownlineLinkRouteValues(model))
            {
                ["IsDescendingSort"] = IsDescendingSort,
                ["SortPropertyName"] = SortPropertyName,
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName,
                ["PageIndex"] = PageIndex,
                ["PageSize"] = PageSize
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(WinLossDetailModel model)
             => new Dictionary<string, object>(base.GetBetListLinkRouteValues(model))
             {
                 ["CustId"] = model.CustId,
                 ["Username"] = model.CustName
             };
    }
}