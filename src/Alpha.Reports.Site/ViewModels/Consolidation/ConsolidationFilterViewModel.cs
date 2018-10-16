namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class ConsolidationFilterViewModel
    {
        public ConsolidationFilterViewModel(
           int custId,
           string custName,
           bool visible,
           DateTime minDate,
           UserLevel custLevel,
           DateRangePickerViewModel dateRangePickerViewModel,
           ProductMultiSelectionViewModel productMultiSelectionViewModel,
           CurrencySelectionViewModel currencySelectionViewModel,
           ExcludeProductViewModel excludeProductViewModel)
        {
            CustId = custId;
            CustName = custName;
            Visible = visible;
            CustLevel = custLevel;
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash());

            DateRangePickerViewModel = dateRangePickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            CurrencySelectionViewModel = currencySelectionViewModel;
            ExcludeProductViewModel = excludeProductViewModel;
        }

        public int CustId { get; }

        public string CustName { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public CurrencySelectionViewModel CurrencySelectionViewModel { get; }

        public int UserSelectedCurrencyId { get; }

        public ExcludeProductViewModel ExcludeProductViewModel { get; }
    }
}