namespace Alpha.Reports.Site.ViewModels.WinLossAnalysis
{
    public class WinLossAnalysisReportType
    {
        public WinLossAnalysisReportType(
            int reportTypeId,
            string reportTypeName,
            bool isChecked)
        {
            ReportTypeId = reportTypeId;
            ReportTypeName = reportTypeName;
            IsChecked = isChecked;
        }

        public int ReportTypeId { get; }

        public string ReportTypeName { get; }

        public bool IsChecked { get; set; }
    }
}