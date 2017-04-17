namespace Alpha.Reports.Site.ViewModels.PlayForFun
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class PlayForFunFilterViewModel
    {
        public PlayForFunFilterViewModel(
            int custId,
            string custName,
            bool visible,
            string breadcrumbLevelJson,
            DateTime minDate,
            UserLevel custLevel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            StatusSelectionViewModel statusSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel)
        {
            CustId = custId;
            CustName = custName;
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash());
            Visible = visible;
            BreadcrumbLevelJson = breadcrumbLevelJson;
            CustLevel = custLevel;

            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            StatusSelectionViewModel = statusSelectionViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
        }

        public int CustId { get; }

        public string CustName { get; }

        public bool Visible { get; }

        public string ViewDataInstructionMessage { get; }

        public string BreadcrumbLevelJson { get; }

        public UserLevel CustLevel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public StatusSelectionViewModel StatusSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }
    }
}