namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using Alpha.Reports.Configurations;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class WinLossFilterViewModel
    {
        public WinLossFilterViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            bool isHistoricReport,
            DateRangePickerViewModel dateRangePickerViewModel,
            MonthPickerViewModel monthPickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            ProductDetailMultiSelectionViewModel productDetailMultiSelectionViewModel,
            bool visible,
            DateTime minDate,
            string breadcrumbLevelJson,
            byte levelType)
        {
            CustId = custId;
            CustName = custName;
            IsHistoricReport = isHistoricReport;
            DateRangePickerViewModel = dateRangePickerViewModel;
            MonthPickerViewModel = monthPickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            ProductDetailMultiSelectionViewModel = productDetailMultiSelectionViewModel;
            ViewDataInstructionMessage = string.Format(
               IsHistoricReport
              ? Resources.CanOnlyViewDataAfterTimeHistoryReport
              : Resources.CanOnlyViewDataAfterTimeCurrentReport,
            minDate.ToShortDateWithSlash());
            Visible = visible;
            CustLevel = custLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson;
            LevelType = levelType;
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public bool IsHistoricReport { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public ProductDetailMultiSelectionViewModel ProductDetailMultiSelectionViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public string ViewActionMessage => IsHistoricReport ? Resources.BackToCurrentReport : Resources.ViewHistoryReport;

        public string BreadcrumbLevelJson { get; }

        public byte LevelType { get; }
    }
}