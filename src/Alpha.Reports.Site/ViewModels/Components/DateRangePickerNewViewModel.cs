namespace Alpha.Reports.Site.ViewModels.Components
{
    using System;
    using Alpha.Reports.Utils;

    public class DateRangePickerNewViewModel
    {
        public DateRangePickerNewViewModel(
            DateTime fromDate,
            DateTime toDate,
            DateTime minDate,
            DateTime historicMinDate,
            DateTime? maxDate = null)
        {
            var today = SystemTime.Today;

            FromDate = fromDate;
            ToDate = toDate;
            MinDate = minDate;
            HistoricMinDate = historicMinDate;
            MaxDate = maxDate ?? today.AddDays(1);
            CurrentDate = today;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MinDate { get; }

        public DateTime HistoricMinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime CurrentDate { get; }

        public bool IsShowHistoricReport => MinDate.AddMonths(3) <= CurrentDate;
    }
}