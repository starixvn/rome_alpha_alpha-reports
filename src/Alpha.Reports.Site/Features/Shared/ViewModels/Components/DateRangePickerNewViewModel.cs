namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    using System;
    using Alpha.Reports.Utils;

    public class DateRangePickerNewViewModel
    {
        public DateRangePickerNewViewModel(
            DateTime fromDate,
            DateTime toDate,
            DateTime minDate,
            DateTime maxDate,
            string language)
        {
            var today = SystemTime.Today;
            FromDate = fromDate;
            ToDate = toDate;
            MinDate = minDate;
            MaxDate = maxDate;
            CurrentDate = today;
            Language = language;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime CurrentDate { get; }

        public string Language { get; }
    }
}