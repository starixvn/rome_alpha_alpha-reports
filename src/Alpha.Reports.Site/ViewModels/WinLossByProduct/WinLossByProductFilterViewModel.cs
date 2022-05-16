namespace Alpha.Reports.Site.ViewModels.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Configurations;
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
            ViewDataInstructionMessage = new List<string>
            {
                string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash()),
                string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate)
            };
           Visible = visible;         
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel { get; }

        public bool Visible { get; }

        public List<string> ViewDataInstructionMessage { get; }
    }
}