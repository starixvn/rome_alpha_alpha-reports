namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using App_GlobalResources;
    using Components;
    using Models;
    using Reports.Extensions;

    public class WinLossFilterViewModel
    {
        public WinLossFilterViewModel(
            int custId,
            string custName,
            UserLevel userLevel,
            bool isHictoricReport,
            DateRangePickerViewModel dateRangePickerViewModel,
            MonthPickerViewModel monthPickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            bool isDirectReportViewAction,
            bool visible,
            DateTime minDate,
            string breadcrumbLevels)
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
            IsDirectReportViewAction = isDirectReportViewAction;
            Visible = visible;
            UserLevel = userLevel;
            BreadcrumbLevels = breadcrumbLevels;
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel UserLevel { get; }

        public bool IsHistoricReport { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public bool IsDirectReportViewAction { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : Resources.ViewHistoryReport;

        public string BreadcrumbLevels { get; }
    }
}