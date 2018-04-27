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
            DateTime limitedDate,
            bool isHistoryReport)
        {
            IsHistoryReport = isHistoryReport;

            FromDate = fromDate == DateTime.MinValue ? isHistoryReport ? SystemTime.Today.FirstDayOfMonth() : SystemTime.Today : fromDate;

            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            LimitedDateToShowInfo = limitedDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public bool IsHistoryReport { get; }
    }
}