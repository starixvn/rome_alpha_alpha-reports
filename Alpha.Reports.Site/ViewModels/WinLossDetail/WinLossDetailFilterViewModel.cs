namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;

    public class WinLossDetailFilterViewModel
    {
        public WinLossDetailFilterViewModel(
            int custId,
            bool isHictoricReport,
            DateRangePickerViewModel dateRangePickerViewModel,
            MonthPickerViewModel monthPickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            bool isDirectReportViewAction,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            IsHistoricReport = isHictoricReport;
            DateRangePickerViewModel = dateRangePickerViewModel;
            MonthPickerViewModel = monthPickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ViewDataInstructionMessage = string.Format(
               IsHistoricReport
              ? Resources.CanOnlyViewDataAfterTimeHistoryReport
              : Resources.CanOnlyViewDataAfterTimeCurrentReport,
            minDate.ToShortDateWithSlash());
            IsDirectReportViewAction = isDirectReportViewAction;
            Visible = visible;
        }

        public int CustId { get; }

        public bool IsHistoricReport { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public bool IsDirectReportViewAction { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : Resources.ViewHistoryReport;
    }
}