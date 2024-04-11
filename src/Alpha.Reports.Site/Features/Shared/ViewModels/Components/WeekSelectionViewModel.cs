namespace Alpha.Reports.Site.Features.Shared.ViewModels.Components
{
    using System;

    public class WeekSelectionViewModel
    {
        public WeekSelectionViewModel(
            DateTime fromDate,
            DateTime toDate,
            string display,
            DateTime? selectedFromDate,
            DateTime? selectedToDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Display = display;
            IsActive = selectedFromDate.HasValue
                && selectedToDate.HasValue
                && selectedFromDate.Value == fromDate
                && selectedToDate.Value == toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Display { get; }

        public bool IsActive { get; }

        public bool IsEnable { get; set; }
    }
}