namespace Alpha.Reports.Site.ViewModels.CommissionByBettype
{
    using System;
    using App_GlobalResources;
    using Components;

    public class CommissionByBettypeFilterViewModel
    {
        public CommissionByBettypeFilterViewModel(DateRangePickerViewModel dateRangePicker, DateTime balanceUpDate)
        {
            DateRangePicker = dateRangePicker;

            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, balanceUpDate.ToShortDateString());
        }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePicker { get; }
    }
}