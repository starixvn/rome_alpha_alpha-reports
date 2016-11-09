namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Components;

    public class WinLossDetailFilterViewModel
    {
        public WinLossDetailFilterViewModel(
            int custId,
            bool isHictoricReport,
            DateRangePickerViewModel dateRangePickerViewModel,
            MonthPickerViewModel monthPickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            string viewDataInstructionMessage,
            bool isDirectReportViewAction,
            bool visible)
        {
            CustId = custId;
            IsHistoricReport = isHictoricReport;
            DateRangePickerViewModel = dateRangePickerViewModel;
            MonthPickerViewModel = monthPickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ViewDataInstructionMessage = viewDataInstructionMessage;
            IsDirectReportViewAction = isDirectReportViewAction;
            Visible = visible;
        }

        public int CustId { get; }

        public bool IsHistoricReport { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public string ViewDataInstructionMessage { get; }

        public bool IsDirectReportViewAction { get; }

        public bool Visible { get; }
    }
}   