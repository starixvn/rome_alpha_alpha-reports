namespace Alpha.Reports.Params
{
    using System;
    using Extensions;
    using Utils;

    public abstract class SupportedHistoryReportParam
    {
        protected SupportedHistoryReportParam(
            DateTime fromDate,
            DateTime toDate,
            bool isHistoryReport)
        {
            IsHistoryReport = isHistoryReport;

            if (fromDate == DateTime.MinValue)
            {
                FromDate = isHistoryReport ? DateTimeUtility.Today.FirstDayOfMonth() : DateTimeUtility.Today;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTimeUtility.Today : toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsHistoryReport { get; }
    }
}