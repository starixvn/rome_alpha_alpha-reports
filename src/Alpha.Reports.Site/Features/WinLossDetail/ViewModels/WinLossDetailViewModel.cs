namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Models.Winloss;
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
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<ProductDetailViewModel> productsDetails,
            bool isGroupHeadline = false,
            EnteredSportResult totalEnteredSport = null)
            : base(message, param, breadcrumbLevel, groupProducts, productsDetails)
        {
            Title = GetReportNameByLevel(message.CustLevel);
            PageIndex = message.PageIndex;
            PageSize = message.PageSize;
            ReportData = reportData;

            var canSort = reportData.DataSource != null && reportData.DataSource.Count() > 1;
            SortPropertyName = canSort ? GetSortedProperty(message.CustLevel, message.SortPropertyName) : DefaultVariables.SortPropertyName;
            BetListPath = Helpers.UrlHelper.MapBetListPath("~/WinLoss/Index");
            IsDescendingSort = canSort && param.IsDescendingSort;

            IsGroupHeadline = isGroupHeadline;
            CanViewBetLists = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();
            SSGUID = message.SSGUID;
            SubAccId = message.SubAccId;

            TotalEnteredSport = totalEnteredSport;
            ViewerLevel = message.ViewerLevel;
            IsOfferNewWinLossDetailReport = message.IsOfferNewWinLossDetailReport;
        }

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(ReportData.CurrentPageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public string Title { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsGroupHeadline { get; }

        public PagedReportData<WinLossDetailModel> ReportData { get; }

        public bool CanViewBetLists { get; }

        public string SSGUID { get; }

        public int SubAccId { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        public UserLevel ViewerLevel { get; }

        public bool IsOfferNewWinLossDetailReport { get; }

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
                 ["CustName"] = model.CustName,
                 ["IsGroupHeadline"] = IsGroupHeadline
             };
    }
}