namespace Alpha.Reports.Models
{
    using System;

    public class DateRange
    {
        public DateRange(DateTime? fromDate, DateTime? todate)
        {
            FromDate = fromDate;
            ToDate = todate;
        }

        public DateTime? FromDate { get; }

        public DateTime? ToDate { get; }
    }
}
