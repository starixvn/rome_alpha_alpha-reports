namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    using System;
    using Alpha.Reports.Utils;

    public class DateRangePickerNewViewModel
    {
        public DateRangePickerNewViewModel(
            DateTime fromDate,
            DateTime toDate,
            DateTime historicPivotDate,
            DateTime minDate,
            DateTime? maxDate = null)
        {
            var today = SystemTime.Today;

            FromDate = fromDate;
            ToDate = toDate;
            HistoricPivotDate = historicPivotDate;
            MinDate = minDate;
            MaxDate = maxDate ?? today.AddDays(1);
            CurrentDate = today;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime HistoricPivotDate { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime CurrentDate { get; }
    }
}