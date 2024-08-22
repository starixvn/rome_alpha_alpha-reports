namespace Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels
{
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Models;

    public class DailyWinLossGetDataViewModel
    {
        public DailyWinLossGetDataViewModel(
            UserLevel viewerLevel,
            ReportData<DailyWinLossModel> reportData)
        {
            ViewerLevel = viewerLevel;
            ReportData = reportData;
        }

        public UserLevel ViewerLevel { get; }

        public ReportData<DailyWinLossModel> ReportData { get; }
    }
}