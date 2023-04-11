namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;

    public class WinLossDetailGetDataViewModel
    {
        public WinLossDetailGetDataViewModel(
            PagedReportData<WinLossDetailModel> winLossDetail,
            EnteredSportResult totalEnteredSport)
        {
            WinLossDetail = winLossDetail;
            TotalEnteredSport = totalEnteredSport;
        }

        public PagedReportData<WinLossDetailModel> WinLossDetail { get; }

        public EnteredSportResult TotalEnteredSport { get; }
    }
}