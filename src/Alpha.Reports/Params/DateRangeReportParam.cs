namespace Alpha.Reports.Params
{
    using System;
    using Extensions;
    using Services;

    public abstract class DateRangeReportParam
    {
        protected DateRangeReportParam(
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

        public virtual bool IsValid()
        {
            if (IsHistoryReport)
            {
                return DateTimeManager.Now.AddYears(-3).FirstDayOfYear() <= FromDate
                    && FromDate <= ToDate
                    && ToDate.Year == FromDate.Year
                    && ToDate.Month == FromDate.Month;
            }

            return DateTimeManager.Now.AddMonths(-3).FirstDayOfMonth() <= FromDate
                   && FromDate <= ToDate;
        }
    }
}