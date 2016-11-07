namespace Alpha.Reports.Site.ViewModels
{
    using System;

    public class MonthAndProductFilterViewModel
    {
        public MonthAndProductFilterViewModel(
            bool hideFilter,
            DateTime minDate,
            MonthPickerViewModel monthPicker,
            ProductMultiSelectionViewModel productMultiSelection)
        {
            HideFilter = hideFilter;
            MinDate = minDate;
            MonthPicker = monthPicker;
            ProductMultiSelection = productMultiSelection;
        }

        public bool HideFilter { get; }

        public DateTime MinDate { get; }

        public MonthPickerViewModel MonthPicker { get; }

        public ProductMultiSelectionViewModel ProductMultiSelection { get; }
    }
}