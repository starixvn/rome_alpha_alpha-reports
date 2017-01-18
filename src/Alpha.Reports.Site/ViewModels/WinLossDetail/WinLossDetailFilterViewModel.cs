namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class WinLossDetailFilterViewModel
    {
        public WinLossDetailFilterViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            bool isHictoricReport,
            DateRangePickerViewModel dateRangePickerViewModel,
            MonthPickerViewModel monthPickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            bool visible,
            DateTime minDate,
            string breadcrumbLevelJson)
        {
            CustId = custId;
            CustName = custName;
            IsHistoricReport = isHictoricReport;
            DateRangePickerViewModel = dateRangePickerViewModel;
            MonthPickerViewModel = monthPickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ViewDataInstructionMessage = string.Format(
               IsHistoricReport
              ? Resources.CanOnlyViewDataAfterTimeHistoryReport
              : Resources.CanOnlyViewDataAfterTimeCurrentReport,
            minDate.ToShortDateWithSlash());
            Visible = visible;
            CustLevel = custLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson;
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public bool IsHistoricReport { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : Resources.ViewHistoryReport;

        public string BreadcrumbLevelJson { get; }
    }
}