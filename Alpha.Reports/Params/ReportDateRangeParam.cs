namespace Alpha.Reports.Params
{
    using System.Collections.Generic;
    using Models;

    public class ReportDateRangeParam
    {
        public ReportDateRangeParam(
            string reportPath,
            bool isHistoricReport,
            IEnumerable<ReportRange> reportRangeSettings)
        {
            ReportPath = reportPath;
            IsHistoricReport = isHistoricReport;
            ReportRangeSettings = reportRangeSettings;
        }

        public string ReportPath { get; }

        public bool IsHistoricReport { get; }

        public IEnumerable<ReportRange> ReportRangeSettings { get; }
    }
}
