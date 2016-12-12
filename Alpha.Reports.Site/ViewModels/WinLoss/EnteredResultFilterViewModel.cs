namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using App_GlobalResources;
    using Components;
    using Models;
    using Reports.Extensions;

    public class EnteredResultFilterViewModel
    {
        public EnteredResultFilterViewModel(
            UserLevel userLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            DateTime minDate)
        {
            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
               Resources.CanOnlyViewDataAfterTimeCurrentReport,
            minDate.ToShortDateWithSlash());
            UserLevel = userLevel;
        }

        public UserLevel UserLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public string ViewDataInstructionMessage { get; }

        public string ViewActionMessage => Resources.ViewHistoryReport;
    }
}