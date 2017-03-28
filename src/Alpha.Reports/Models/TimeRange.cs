using System;

namespace Alpha.Reports.Models
{
    public class TimeRange
    {
        public TimeRange(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
