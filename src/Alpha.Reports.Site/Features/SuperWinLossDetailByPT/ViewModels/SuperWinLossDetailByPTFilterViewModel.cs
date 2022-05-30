namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT.ViewModels
{
    using System;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Site.ViewModels.Components;
    using App_GlobalResources;

    public class SuperWinLossDetailByPTFilterViewModel
    {
        public SuperWinLossDetailByPTFilterViewModel(
            int custId,
            DateRangePickerViewModel dateRangePickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            ProductDetailMultiSelectionViewModel productDetailMultiSelectionViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ProductDetailMultiSelectionViewModel = productDetailMultiSelectionViewModel;

            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());
            Visible = visible;
            SeparateSabaMessage = string.Format(Resources.SabaSportsNote, SiteSettings.SeperateSabaSportsDate);
        }

        public int CustId { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public string SeparateSabaMessage { get; }
    }
}