namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using System;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;

    public class MatchWinLossDetailFilterViewModel
    {
        public MatchWinLossDetailFilterViewModel(
            int custId,
            bool visible,
            UserLevel custLevel,
            DateRangePickerViewModel dateRangePickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel,
            LeagueMultiSelectionViewModel leagueMultiSelectionViewModel,
            DateTime minDate)
        {
            CustId = custId;
            Visible = visible;
            CustLevel = custLevel;

            DateRangePickerViewModel = dateRangePickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
            LeagueMultiSelectionViewModel = leagueMultiSelectionViewModel;
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, minDate.ToShortDateWithSlash());
        }

        public int CustId { get; }

        public string CustName { get; }

        public bool Visible { get; }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public LeagueMultiSelectionViewModel LeagueMultiSelectionViewModel { get; }

        public string ViewDataInstructionMessage { get; }
    }
}