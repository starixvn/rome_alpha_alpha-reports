namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class RacingCommissionFilterViewModel
    {
        public RacingCommissionFilterViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            bool visible,
            DateTime balanceUpDate,
            string breadcrumbLevelJson,
            DateRangePickerViewModel dateRangePicker)
        {
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;
            Visible = visible;
            BreadcrumbLevelJson = breadcrumbLevelJson;
            DateRangePicker = dateRangePicker;

            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, balanceUpDate.ToShortDateWithSlash());
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePicker { get; }

        public string BreadcrumbLevelJson { get; }
    }
}