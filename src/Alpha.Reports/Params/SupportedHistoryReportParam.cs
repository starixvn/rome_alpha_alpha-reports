namespace Alpha.Reports.Params
{
    using System;
    using Extensions;
    using Services;

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
                FromDate = isHistoryReport ? DateTimeManager.Now.FirstDayOfMonth() : DateTimeManager.Now;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTimeManager.Now : toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsHistoryReport { get; }
    }
}