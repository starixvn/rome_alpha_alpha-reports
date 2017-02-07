namespace Alpha.Reports.Site.ViewModels.WinLossByBettype
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class WinLossByBettypeFilterViewModel
    {
        public WinLossByBettypeFilterViewModel(
            int custId,
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            CustLevel = custLevel;

            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());
           Visible = visible;         
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }
    }
}