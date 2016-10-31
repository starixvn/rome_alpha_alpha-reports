namespace Alpha.Reports.Models
{
    using System;

    public class DateRange
    {
        public DateRange(DateTime minDate, DateTime maxDate)
        {
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }
    }
}
