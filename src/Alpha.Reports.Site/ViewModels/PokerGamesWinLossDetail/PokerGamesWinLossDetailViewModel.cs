namespace Alpha.Reports.Site.ViewModels.PokerGamesWinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using Components;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using Requests.PokerGamesWinLossDetail;

    public class PokerGamesWinLossDetailViewModel : WinLossReportBaseViewModel<PokerGamesWinLossDetailModel>
    {
        public PokerGamesWinLossDetailViewModel(
            PokerGamesWinLossDetailQuery message,
            PokerGamesWinLossDetailParam param,
            PagedReportData<PokerGamesWinLossDetailModel> reportData,
            IEnumerable<CustInfo> breadcrumbLevel,
            bool isGroupHeadline = false)
            : base(message, param, breadcrumbLevel, null)
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
        }

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(ReportData.CurrentPageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public string Title { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsGroupHeadline { get; }

        public PagedReportData<PokerGamesWinLossDetailModel> ReportData { get; }

        private string GetReportNameByLevel(UserLevel userLevel)
            => userLevel.IsSuperLevel()
            ? Resources.MasterPokerGamesWinLossDetail
            : userLevel.IsMasterLevel()
                ? Resources.AgentPokerGamesWinLossDetail
                : Resources.MemberPokerGamesWinLossDetail;

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(PokerGamesWinLossDetailModel model)
            => new Dictionary<string, object>(base.GetDownlineLinkRouteValues(model))
            {
                ["IsDescendingSort"] = IsDescendingSort,
                ["SortPropertyName"] = SortPropertyName,
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName,
                ["PageIndex"] = PageIndex,
                ["PageSize"] = PageSize
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(PokerGamesWinLossDetailModel model)
             => new Dictionary<string, object>(base.GetBetListLinkRouteValues(model))
             {
                 ["CustId"] = model.CustId,
                 ["CustName"] = model.CustName,
                 ["IsGroupHeadline"] = IsGroupHeadline
             };
    }
}