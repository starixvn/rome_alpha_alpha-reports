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
            ProductDetailMultiSelectionViewModel productDetailMultiSelectionViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            CustLevel = custLevel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ProductDetailMultiSelectionViewModel = productDetailMultiSelectionViewModel;

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

        public ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }
    }
}