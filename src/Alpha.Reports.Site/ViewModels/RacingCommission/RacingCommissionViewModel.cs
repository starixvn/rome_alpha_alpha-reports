namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using App_GlobalResources;
    using Components;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.RacingCommission;
    using Requests.RacingCommission;

    public class RacingCommissionViewModel : DrillDownableReportViewModel<RacingCommissionModel>
    {
        public const string SortByCustName = nameof(RacingCommissionModel.CustName);
        public const string SortByToteWin = nameof(RacingCommissionModel.ToteWin);
        public const string SortByTotePlace = nameof(RacingCommissionModel.TotePlace);
        public const string SortByToteWinPlace = nameof(RacingCommissionModel.ToteWinPlace);
        public const string SortByTotal = nameof(RacingCommissionModel.Total);

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

            RacingCommissionFilter = new RacingCommissionFilterViewModel(
                query.CustId,
                query.CustName,
                query.CustLevel,
                query.IsFilterVisible,
                balanceUpDate,
                BreadcrumbLevelJson,
                dateRangePickerViewModel);
            RacingCommissions = racingCommissions;

            var canSort = racingCommissions != null && racingCommissions.Count() > 1;
            SortPropertyName = canSort && !string.IsNullOrWhiteSpace(query.SortPropertyName)
                ? query.SortPropertyName
                : DefaultVariables.SortPropertyName;
            IsDescendingSort = canSort && query.IsDescendingSort;
            BetListPath = Helpers.UrlHelper.MapBetListPath("~/WinLoss/Index");
        }

        public RacingCommissionQuery Query { get; }

        public string Title => Resources.RacingCommission;

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public RacingCommissionFilterViewModel RacingCommissionFilter { get; }

        public IEnumerable<RacingCommissionModel> RacingCommissions { get; }

        public RacingCommissionModel Total => RacingCommissionModel.GetAccumulatedCommission(RacingCommissions);

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(RacingCommissionModel model)
            => new Dictionary<string, object>
            {
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(RacingCommissionModel model)
            => new Dictionary<string, object>
            {
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName,
                ["FromDate"] = Query.FromDate,
                ["ToDate"] = Query.ToDate,
                ["ProductIds"] = ProductIdConstants.Racing.ToString()
            };
    }
}