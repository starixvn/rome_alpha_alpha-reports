namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailGetDataViewModel : WinLossDetailGetDataWeekViewModel
    {
        public WinLossDetailGetDataViewModel(
            WinLossDetailGetDataQuery message,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail) : base(message, totalEnteredSport, winLossDetail)
        {
        }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }
    }
}