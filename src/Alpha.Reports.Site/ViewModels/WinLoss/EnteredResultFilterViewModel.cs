namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class EnteredResultFilterViewModel
    {
        public EnteredResultFilterViewModel(
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            DateTime minDate)
        {
            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
               Resources.CanOnlyViewDataAfterTimeCurrentReport,
            minDate.ToShortDateWithSlash());
            CustLevel = custLevel;
        }

        public UserLevel CustLevel { get; }

        public bool Visible { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public string ViewDataInstructionMessage { get; }

        public string ViewActionMessage => Resources.ViewHistoryReport;
    }
}