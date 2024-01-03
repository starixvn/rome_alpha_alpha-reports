namespace Alpha.Reports.Site.ViewModels.MatchWinLoss
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Configurations;
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
            ViewDataInstructionMessage = new List<string>
            {
                string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash())
            };
            Visible = visible;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public bool Visible { get; }

        public List<string> ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }
    }
}