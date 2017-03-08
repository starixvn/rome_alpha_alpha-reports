namespace Alpha.Reports.Site.ViewModels.CommissionByBettype
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;

    public class CommissionByBettypeFilterViewModel
    {
        public CommissionByBettypeFilterViewModel(DateRangePickerViewModel dateRangePicker, DateTime balanceUpDate)
        {
            DateRangePicker = dateRangePicker;

            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, balanceUpDate.ToShortDateWithSlash());
        }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePicker { get; }
    }
}