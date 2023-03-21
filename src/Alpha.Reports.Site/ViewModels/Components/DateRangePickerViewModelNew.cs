namespace Alpha.Reports.Site.ViewModels.Components
{
    using System;
    using Alpha.Reports.Utils;

    public class DateRangePickerViewModelNew
    {
        public DateRangePickerViewModelNew(
            DateTime minDate,
            DateTime maxDate,
            DateTime fromDate,
            DateTime toDate)
        {
            MinDate = minDate;
            MaxDate = maxDate;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime CurrentDate { get; } = SystemTime.Today;
    }
}