namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailViewModelNew
    {
        public WinLossDetailViewModelNew(
            WinLossDetailQueryNew message,
            PagedReportData<WinLossDetailModel> winLossDetail,
            EnteredSportResult totalEnteredSport)
        {
            IsProtectedMode = message.IsProtectedMode;
            WinLossDetail = winLossDetail;
            TotalEnteredSport = totalEnteredSport;
        }

        public bool IsProtectedMode { get; }

        public PagedReportData<WinLossDetailModel> WinLossDetail { get; }

        public EnteredSportResult TotalEnteredSport { get; }

    }
}