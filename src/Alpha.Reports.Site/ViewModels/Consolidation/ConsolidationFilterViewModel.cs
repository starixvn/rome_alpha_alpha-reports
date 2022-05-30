namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Configurations;
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
           ProductDetailMultiSelectionViewModel productDetailMultiSelectionViewModel,
           CurrencySelectionViewModel currencySelectionViewModel)
        {
            CustId = custId;
            CustName = custName;
            Visible = visible;
            CustLevel = custLevel;
            ViewDataInstructionMessage = new List<string>
            {
                string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash()),
                string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate)
            };

            DateRangePickerViewModel = dateRangePickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ProductDetailMultiSelectionViewModel = productDetailMultiSelectionViewModel;
            CurrencySelectionViewModel = currencySelectionViewModel;
        }

        public int CustId { get; }

        public string CustName { get; }

        public bool Visible { get; }

        public List<string> ViewDataInstructionMessage { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel { get; }

        public CurrencySelectionViewModel CurrencySelectionViewModel { get; }

        public int UserSelectedCurrencyId { get; }
    }
}