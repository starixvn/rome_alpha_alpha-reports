namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using App_GlobalResources;
    using Components;

    public class RacingCommissionFilterViewModel
    {
        public RacingCommissionFilterViewModel(DateRangePickerViewModel dateRangePicker, DateTime balanceUpDate)
        {
            DateRangePicker = dateRangePicker;

            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, balanceUpDate.ToShortDateString());
        }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePicker { get; }
    }
}