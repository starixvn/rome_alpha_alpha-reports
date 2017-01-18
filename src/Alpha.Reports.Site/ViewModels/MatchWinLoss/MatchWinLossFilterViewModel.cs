namespace Alpha.Reports.Site.ViewModels.MatchWinLoss
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class MatchWinLossFilterViewModel
    {
        public MatchWinLossFilterViewModel(
            int custId,
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            bool visible,
            DateTime minDate)
        {
            CustId = custId;
            CustLevel = custLevel;
            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash());
            Visible = visible;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }
    }
}