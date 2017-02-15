namespace Alpha.Reports.Site.ViewModels.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Reports.Models;
    using Reports.Models.CommissionByBettype;
    using Requests.CommissionByBettype;

    public class CommissionByBettypeViewModel : DrillDownableReportViewModel<CommissionByBettypeModel>
    {
        public CommissionByBettypeViewModel(
            CommissionByBettypeQuery query,
            IEnumerable<CommissionByBettypeModel> commissionByBettypes,
            IEnumerable<CustInfo> breadcrumbLevel,
            DateTime balanceUpDate,
            bool showChargeCommColumn,
            bool showMyanmarOddsColumn,
            bool showOthersCommColumn)
            : base(query.CustLevel, breadcrumbLevel)
        {
            Query = query;
            CommissionByBettypes = commissionByBettypes;

            DateRangePickerViewModel dateRangePickerViewModel = new DateRangePickerViewModel(
              balanceUpDate,
              DateTime.MaxValue,
              query.FromDate,
              query.ToDate,
              query.UserSelectedDateRangeOption);

            CommissionByBettypeFilter = new CommissionByBettypeFilterViewModel(dateRangePickerViewModel, balanceUpDate);

            ShowChargeCommColumn = showChargeCommColumn;
            ShowMyanmarOddsColumn = showMyanmarOddsColumn;
            ShowOthersCommColumn = showOthersCommColumn;
        }

        public CommissionByBettypeQuery Query { get; }

        public string Title => Resources.CommissionByBettype;

        public CommissionByBettypeFilterViewModel CommissionByBettypeFilter { get; }

        public IEnumerable<CommissionByBettypeModel> CommissionByBettypes { get; }

        public CommissionByBettypeModel Total => CommissionByBettypeModel.GetAccumulatedCommission(CommissionByBettypes);

        public bool ShowChargeCommColumn { get; }

        public bool ShowMyanmarOddsColumn { get; }

        public bool ShowOthersCommColumn { get; }

        public int GetNumCols()
        {
            int numCols = 5;
            if (ShowChargeCommColumn)
            {
                numCols++;
            }

            if (ShowMyanmarOddsColumn)
            {
                numCols++;
            }

            if (ShowOthersCommColumn)
            {
                numCols++;
            }

            return numCols;
        }

        protected override IDictionary<string, object> GetBetListLinkRouteValues(CommissionByBettypeModel model)
        {
            throw new NotImplementedException();
        }

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(CommissionByBettypeModel model) => new Dictionary<string, object>
        {
            { "CustId", model.CustId },
            { "CustName", model.CustName }
        };
    }
}