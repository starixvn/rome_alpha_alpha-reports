namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.Helpers;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailGetDataWeekViewModel
    {
        public WinLossDetailGetDataWeekViewModel(
            WinLossDetailGetDataWeekQuery message,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail)
        {
            CustLevel = message.CustLevel;
            CanViewBetLists = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();
            TotalEnteredSport = totalEnteredSport;
            WinLossDetail = winLossDetail;
            BreadcrumbLevelJson = JsonHelper.ToJson(message.BreadcrumbLevel);
            CanShowMonitoredIcon = !message.IsProtectedMode && message.ViewerLevel.IsAgentLevel();
            CanUpdateAccount = message.CanUpdateAccount;
            CurrencyName = message.CurrencyName;
        }

        public UserLevel CustLevel { get; }

        public bool CanViewBetLists { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        public PagedReportData<WinLossDetailModel> WinLossDetail { get; }

        public string BreadcrumbLevelJson { get; }

        public bool CanShowMonitoredIcon { get; }

        public bool CanUpdateAccount { get; }

        public string CurrencyName { get; }
    }
}