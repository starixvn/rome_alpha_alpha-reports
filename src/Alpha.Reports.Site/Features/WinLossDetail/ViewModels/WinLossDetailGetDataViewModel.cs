namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.Helpers;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailGetDataViewModel
    {
        public WinLossDetailGetDataViewModel(
            WinLossDetailGetDataQuery message,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail)
        {
            CustLevel = message.CustLevel;
            CanViewBetLists = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();
            BreadcrumbLevelJson = JsonHelper.ToJson(message.BreadcrumbLevel);
            TotalEnteredSport = totalEnteredSport;
            WinLossDetail = winLossDetail;
        }

        public UserLevel CustLevel { get; }

        public bool CanViewBetLists { get; }

        public string BreadcrumbLevelJson { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        public PagedReportData<WinLossDetailModel> WinLossDetail { get; }
    }
}