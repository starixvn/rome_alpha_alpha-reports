namespace Alpha.Reports.Site.ViewModels.Components
{
    using System;

    public class MonthPickerViewModel
    {
        public MonthPickerViewModel(DateTime minDate, DateTime fromDate, DateTime toDate)
        {
            MinDate = minDate;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime MinDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}