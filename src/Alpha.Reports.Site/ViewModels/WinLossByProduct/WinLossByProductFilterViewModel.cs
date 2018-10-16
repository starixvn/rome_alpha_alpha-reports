namespace Alpha.Reports.Site.ViewModels.WinLossByProduct
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class WinLossByProductFilterViewModel
    {
        public WinLossByProductFilterViewModel(
            int custId,
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            ExcludeProductViewModel excludeProductViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            CustLevel = custLevel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ExcludeProductViewModel = excludeProductViewModel;

            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());
           Visible = visible;         
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public ExcludeProductViewModel ExcludeProductViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }
    }
}