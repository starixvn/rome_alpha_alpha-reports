namespace Alpha.Reports.Site.ViewModels.WinLossAnalysis
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class WinLossAnalysislFilterViewModel
    {
        public WinLossAnalysislFilterViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            bool visible,
            DateTime minDate,
            IEnumerable<WinLossAnalysisReportTypeViewModel> reportTypeData)
        {
            CustId = custId;
            CustName = custName;
            DateRangePickerViewModel = dateRangePickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());
            Visible = visible;
            CustLevel = custLevel;
            ReportTypeData = reportTypeData;
        }

        public IEnumerable<WinLossAnalysisReportTypeViewModel> ReportTypeData { get; private set; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }
    }
}