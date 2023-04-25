namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;

    public class WinLossDetailGetDataViewModel
    {
        public WinLossDetailGetDataViewModel(
            UserLevel custLevel,
            bool canViewBetLists,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail)
        {
            CustLevel = custLevel;
            CanViewBetLists = canViewBetLists;
            TotalEnteredSport = totalEnteredSport;
            WinLossDetail = winLossDetail;
        }

        public UserLevel CustLevel { get; }

        public bool CanViewBetLists { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        public PagedReportData<WinLossDetailModel> WinLossDetail { get; }
    }
}