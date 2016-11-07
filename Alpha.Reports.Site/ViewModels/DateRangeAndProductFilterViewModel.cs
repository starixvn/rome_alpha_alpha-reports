namespace Alpha.Reports.Site.ViewModels
{
    using System;

    public class DateRangeAndProductFilterViewModel
    {
        public DateRangeAndProductFilterViewModel(
            bool hideFilter,
            DateTime minDate,
            DateRangePickerViewModel dateRangePicker,
            ProductMultiSelectionViewModel productMultiSelection)
        {
            HideFilter = hideFilter;
            MinDate = minDate;
            DateRangePicker = dateRangePicker;
            ProductMultiSelection = productMultiSelection;
        }

        public bool HideFilter { get; }

        public DateTime MinDate { get; }

        public DateRangePickerViewModel DateRangePicker { get; }

        public ProductMultiSelectionViewModel ProductMultiSelection { get; }
    }
}