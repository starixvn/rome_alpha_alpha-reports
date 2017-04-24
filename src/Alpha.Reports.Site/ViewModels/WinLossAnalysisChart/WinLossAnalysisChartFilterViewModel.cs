namespace Alpha.Reports.Site.ViewModels.WinLossAnalysisChart
{
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;
    using System;

    public class WinLossAnalysisChartFilterViewModel
    {
        public WinLossAnalysisChartFilterViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            MonthPickerViewModel monthPickerViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            MonthPickerViewModel = monthPickerViewModel;
            Visible = visible;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }
    }
}