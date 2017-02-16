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
                FromDate = isHistoryReport ? SystemTime.Today.FirstDayOfMonth() : SystemTime.Today;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsHistoryReport { get; }
    }
}