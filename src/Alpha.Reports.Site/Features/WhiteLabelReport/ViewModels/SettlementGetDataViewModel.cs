namespace Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels
{
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Models;

    public class SettlementGetDataViewModel
    {
        public SettlementGetDataViewModel(
            UserLevel viewerLevel,
            ReportData<SettlementModel> reportData)
        {
            ViewerLevel = viewerLevel;
            ReportData = reportData;
        }

        public UserLevel ViewerLevel { get; }

        public ReportData<SettlementModel> ReportData { get; }
    }
}