namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using System.Collections.Generic;
    using Components;
    using Reports.Models;
    using Reports.Models.RacingCommission;
    using Requests.RacingCommission;

    public class RacingCommissionViewModel : DrillDownableReportViewModel<RacingCommissionModel>
    {
        public RacingCommissionViewModel(
            RacingCommissionQuery query,
            IEnumerable<RacingCommissionModel> racingCommission,
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
            RacingCommissions = racingCommission;
        }

        public string Title
        {
            get
            {
                return "Racing Commission";
            }
        }

        public RacingCommissionQuery Query { get; }

        public RacingCommissionFilterViewModel RacingCommissionFilter { get; }

        public IEnumerable<RacingCommissionModel> RacingCommissions { get; }

        public RacingCommissionModel Total => RacingCommissionModel.GetAccumulatedCommission(RacingCommissions);

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(RacingCommissionModel model) => new Dictionary<string, object>
        {
            { nameof(RacingCommissionModel.CustId), model.CustId },
            { nameof(RacingCommissionModel.CustName), model.CustName }
        };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(RacingCommissionModel model) => new Dictionary<string, object>
        {
            { nameof(RacingCommissionModel.CustId), model.CustId },
            { nameof(RacingCommissionModel.CustName), model.CustName },
            { nameof(RacingCommissionQuery.FromDate),  Query.FromDate },
            { nameof(RacingCommissionQuery.ToDate),  Query.ToDate },
            { nameof(RacingCommissionQuery.UserSelectedDateRangeOption),  Query.UserSelectedDateRangeOption }
        };
    }
}