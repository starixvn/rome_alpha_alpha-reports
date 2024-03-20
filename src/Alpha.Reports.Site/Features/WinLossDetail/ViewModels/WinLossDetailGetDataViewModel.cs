namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class WinLossDetailGetDataViewModel : WinLossDetailGetDataWeekViewModel
    {
        public WinLossDetailGetDataViewModel(
            WinLossDetailGetDataQuery message,
            EnteredSportResult totalEnteredSport,
            PagedReportData<WinLossDetailModel> winLossDetail,
            IEnumerable<WeekSelectionViewModel> weeks)
            : base(message, totalEnteredSport, winLossDetail)
        {
            Weeks = weeks;
        }

        public IEnumerable<WeekSelectionViewModel> Weeks { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }
    }
}