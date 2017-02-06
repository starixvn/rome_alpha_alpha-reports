namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Reports.Models;
    using Reports.Models.RacingCommission;
    using Requests.RacingCommission;

    public class RacingCommissionViewModel : DrillDownableReportViewModel<RacingCommissionModel>
    {
        public RacingCommissionViewModel(
            RacingCommissionQuery query,
            IEnumerable<RacingCommissionModel> racingCommissions,
            DateTime balanceUpDate,
            IEnumerable<CustInfo> breadcrumbLevel)
            : base(query.CustLevel, breadcrumbLevel)
        {
            Query = query;

            DateRangePickerViewModel dateRangePickerViewModel = new DateRangePickerViewModel(
                balanceUpDate,
                DateTime.MaxValue,
                query.FromDate,
                query.ToDate,
                query.UserSelectedDateRangeOption);

            RacingCommissionFilter = new RacingCommissionFilterViewModel(dateRangePickerViewModel, balanceUpDate);
            RacingCommissions = racingCommissions;
        }

        public RacingCommissionQuery Query { get; }

        public string Title => Resources.RacingCommission;

        public RacingCommissionFilterViewModel RacingCommissionFilter { get; }

        public IEnumerable<RacingCommissionModel> RacingCommissions { get; }

        public RacingCommissionModel Total => RacingCommissionModel.GetAccumulatedCommission(RacingCommissions);

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(RacingCommissionModel model) => new Dictionary<string, object>
        {
            { "CustId", model.CustId },
            { "CustName", model.CustName },
        };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(RacingCommissionModel model) => new Dictionary<string, object>
        {
            { "CustId", model.CustId },
            { "CustName", model.CustName },
            { "Type", "WLByDate" },
            { "FDate",  Query.FromDate },
            { "TDate",  Query.ToDate },
            { "ProductIds", "2" },
            { "FilterPostback", 1 },
            { "IsHistoryReport", 0 }
        };
    }
}