namespace Alpha.Reports.Site.ViewModels.WinLossAnalysis
{
    public class WinLossAnalysisReportType
    {
        public WinLossAnalysisReportType(
            int reportTypeId,
            string reportTypeName,
            bool isChecked,
            string reportTypeTitle)
        {
            ReportTypeId = reportTypeId;
            ReportTypeName = reportTypeName;
            IsChecked = isChecked;
            ReportTypeTitlte = reportTypeTitle;
        }

        public int ReportTypeId { get; }

        public string ReportTypeName { get; }

        public bool IsChecked { get; set; }

        public string ReportTypeTitlte { get; set; }
    }
}