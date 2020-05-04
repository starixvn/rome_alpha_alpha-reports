namespace Alpha.Reports.Site.ViewModels.PlayForFun
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Components;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Requests.PlayForFun;

    public class PlayForFunReportViewModel : DrillDownableReportViewModel<PlayForFunModel>
    {
        public const string SortByCustName = nameof(PlayForFunModel.CustName);
        public const string SortByTurnover = nameof(PlayForFunModel.Turnover);
        public const string SortByMemberWinLoss = nameof(PlayForFunModel.WinLoss);
        public const string SortByPlayCount = nameof(PlayForFunModel.BetCount);

        public static readonly Dictionary<int, string> BreadcrumbActionsDictionary = new Dictionary<int, string>
        {
            [4] = "masteroragent",
            [3] = "masteroragent",
            [2] = "member"
        };

        private readonly PlayForFunQuery query;
        private readonly PlayForFunParam param;

        public PlayForFunReportViewModel(
            PlayForFunQuery query,
            PlayForFunParam param,
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<SelectListItem> statuses,
            ReportData<PlayForFunModel> reportData)
            : base(query.CustLevel, breadcrumbLevel)
        {
            this.query = query;
            this.param = param;

            GroupProducts = groupProducts;
            Statuses = statuses;
            ReportData = reportData;

            BreadcrumbActions = breadcrumbLevel.Select(b => BreadcrumbActionsDictionary[b.CustLevelId]);
        }

        public int CustId => query.CustId;

        public string CustName => query.CustName;

        public DateTime FromDate => query.FromDate;

        public DateTime ToDate => query.ToDate;

        public DateTime MinDate => query.MinDate;

        public DateTime MaxDate => query.MaxDate;

        public string SortPropertyName { get; protected set; }

        public bool IsDescendingSort { get; protected set; }

        public int UserSelectedProductId => param.ProductId;

        public int UserSelectedStatusId => query.UserSelectedStatusId;

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public IEnumerable<SelectListItem> Statuses { get; }

        public ReportData<PlayForFunModel> ReportData { get; }

        public IEnumerable<string> BreadcrumbActions { get; }

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(GroupProducts, query.UserSelectedProductId.ToString(), query.Language);

        private StatusSelectionViewModel StatusSelectionViewModel =>
            new StatusSelectionViewModel(Statuses, query.UserSelectedStatusId.ToString());

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, query.FromDate, query.ToDate, query.UserSelectedDateRangeOption);

        public PlayForFunFilterViewModel FilterViewModel =>
            new PlayForFunFilterViewModel(
                query.CustId,
                query.CustName,
                query.IsFilterVisible,
                BreadcrumbLevelJson,
                MinDate,
                query.CustLevel,
                ProductMultiSelectionViewModel,
                StatusSelectionViewModel,
                DateRangePickerViewModel);

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(PlayForFunModel model)
            => new Dictionary<string, object>
            {
                ["IsFilterVisible"] = query.IsFilterVisible,
                ["IsDescendingSort"] = query.IsDescendingSort,
                ["SortPropertyName"] = SortPropertyName,
                ["IsDescendingSort"] = IsDescendingSort,
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName,
                ["UserSelectedProductId"] = query.UserSelectedProductId,
                ["UserSelectedStatusId"] = query.UserSelectedStatusId,
                ["UserSelectedDateRangeOption"] = query.UserSelectedDateRangeOption,
                ["FromDate"] = query.FromDate,
                ["ToDate"] = query.ToDate
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(PlayForFunModel model)
            => new Dictionary<string, object>();
    }
}