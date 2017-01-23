namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using Components;

    public class RacingCommissionFilterViewModel
    {
        public RacingCommissionFilterViewModel(DateRangePickerViewModel dateRangePicker, DateTime balanceUpDate)
        {
            DateRangePicker = dateRangePicker;

            ViewDataInstructionMessage = $"You can view the report data from {balanceUpDate.ToShortDateString()}";
        }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePicker { get; }
    }
}