namespace Alpha.Reports.Site.ViewModels.WinLossByGameType
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class WinLossByGameTypeFilterViewModel
    {
        public WinLossByGameTypeFilterViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;

            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());
           Visible = visible;         
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }
    }
}