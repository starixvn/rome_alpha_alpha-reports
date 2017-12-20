namespace Alpha.Reports.Models
{
    using System;

    public class TimeRange
    {
        public TimeRange()
        {

        }

        public TimeRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
