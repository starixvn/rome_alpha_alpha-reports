namespace Alpha.Reports.Site.ViewModels.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using Components;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.CommissionByBettype;
    using Requests.CommissionByBettype;

    public class CommissionByBettypeViewModel : DrillDownableReportViewModel<CommissionByBettypeModel>
    {
        public const string SortByCustName = nameof(CommissionByBettypeModel.CustName);
        public const string SortByIsDirectMember = nameof(CommissionByBettypeModel.IsDirectMember);
        public const string SortByComm1 = nameof(CommissionByBettypeModel.Comm1);
        public const string SortByComm2 = nameof(CommissionByBettypeModel.Comm2);
        public const string SortByComm3 = nameof(CommissionByBettypeModel.Comm3);
        public const string SortByCasinoComm = nameof(CommissionByBettypeModel.CasinoComm);

        public CommissionByBettypeViewModel(
            CommissionByBettypeQuery query,
            IEnumerable<CommissionByBettypeModel> commissionByBettypes,
            IEnumerable<CustInfo> breadcrumbLevel,
            DateTime balanceUpDate)
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

            CommissionByBettypeFilter = new CommissionByBettypeFilterViewModel(
                query.CustId,
                query.CustName,
                query.CustLevel,
                query.IsFilterVisible,
                balanceUpDate,
                BreadcrumbLevelJson,
                dateRangePickerViewModel,
                query.LevelType);

            var canSort = commissionByBettypes != null && commissionByBettypes.Count() > 1;
            SortPropertyName = canSort && !string.IsNullOrWhiteSpace(query.SortPropertyName)
                ? query.SortPropertyName
                : DefaultVariables.SortPropertyName;
            IsDescendingSort = canSort && query.IsDescendingSort;
            ShowLevelType = query.CustLevel.IsNotAgentLevel();
            LevelTypeValue = query.CustLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent;
        }

        public CommissionByBettypeQuery Query { get; }

        public string Title => Resources.CommissionByBettype;

        public CommissionByBettypeFilterViewModel CommissionByBettypeFilter { get; }

        public IEnumerable<CommissionByBettypeModel> CommissionByBettypes { get; }

        public CommissionByBettypeModel Total => CommissionByBettypeModel.GetAccumulatedCommission(CommissionByBettypes);

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool ShowLevelType { get; }

        public string LevelTypeValue { get; }

        public int GetNumCols()
        {
            var numCols = 5;

            if (ShowLevelType)
            {
                numCols++;
            }

            return numCols;
        }

        protected override IDictionary<string, object> GetBetListLinkRouteValues(CommissionByBettypeModel model)
        {
            throw new NotImplementedException();
        }

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(CommissionByBettypeModel model)
            => new Dictionary<string, object>
            {
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName
            };
    }
}