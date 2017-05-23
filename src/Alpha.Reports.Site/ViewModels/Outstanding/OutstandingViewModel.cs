namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Enumerations;
    using Extensions;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Requests.Outstanding;

    public class OutstandingViewModel : DrillDownableReportViewModel<OutstandingModel>
    {
        public const string CustNamePropertyName = nameof(OutstandingModel.CustName);

        // Always need to show member outstanding
        private readonly List<UserLevel> displayOutstandingLevels = new List<UserLevel> { UserLevel.Member };

        public OutstandingViewModel(
            OutstandingQuery query,
            IEnumerable<OutstandingModel> outstandingDataSource,
            IEnumerable<int> selectedProductIds,
            IEnumerable<SelectListItem> products,
            IEnumerable<CustInfo> breadcrumbLevel)
               : base(query.CustLevel, breadcrumbLevel)
        {
            Query = query;
            OutstandingDatasource = outstandingDataSource;
            ProductMultiSelectionViewModel = new ProductMultiSelectionViewModel(products, selectedProductIds.JoinWithComma(), string.Empty);

            for (var level = (byte)(ReportLevel.Value + 1); level <= Query.ViewerLevel.Value; level++)
            {
                displayOutstandingLevels.Add(Enumeration.FromValue<UserLevel>(level));
            }

            var canSort = outstandingDataSource != null && outstandingDataSource.Count() > 1;
            SortPropertyName = canSort ? GetSortedProperty(query.SortPropertyName) : DefaultVariables.SortPropertyName;
            IsDescendingSort = canSort && query.IsDescendingSort;
            BetListPath = Helpers.UrlHelper.MapBetListPath("~/RunningBetList/Index");
        }

        public string Title
        {
            get
            {
                if (ReportLevel.IsMasterLevel())
                {
                    return Resources.MasterOutstanding;
                }

                if (ReportLevel.IsAgentLevel())
                {
                    return Resources.AgentOutstanding;
                }

                return Resources.MemberOutstanding;
            }
        }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        public bool IsFilterVisible => Query.IsFilterVisible;

        public OutstandingQuery Query { get; }

        public UserLevel ReportLevel => Query.CustLevel.GetDirectDownLineLevel();

        public IEnumerable<OutstandingModel> OutstandingDatasource { get; }

        public OutstandingModel Total => OutstandingModel.GetAccumulatedData(OutstandingDatasource);

        public List<UserLevel> DisplayOutstandingLevels => displayOutstandingLevels;

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public int NumberOfOutstandingColumn => DisplayOutstandingLevels.Count;

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(OutstandingModel model) => new Dictionary<string, object>
        {
            { "CustId", model.CustId },
            { "CustName", model.CustName },
            { "UserSelectedProductIds", ProductMultiSelectionViewModel.UserSelectedProductIds },
            { "SortPropertyName", SortPropertyName },
            { "IsDescendingSort", IsDescendingSort },
            { "IsFilterVisible", IsFilterVisible }
        };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(OutstandingModel model) => new Dictionary<string, object>
        {
            ["CustId"] = model.CustId,
            ["CustName"] = model.CustName,
            ["GetAll"] = true,
            ["ProductIds"] = ProductMultiSelectionViewModel.UserSelectedProductIds
        };

        private string GetSortedProperty(string sortedProperty)
        {
            if (string.IsNullOrWhiteSpace(sortedProperty))
            {
                return DefaultVariables.SortPropertyName;
            }

            var sortableColumns = displayOutstandingLevels
                .Select(level => (level.DisplayName + "Outstanding").ToUpperInvariant())
                .ToList();
            sortableColumns.Add(CustNamePropertyName);

            return sortableColumns.Contains(sortedProperty.ToUpperInvariant())
                ? sortedProperty
                : DefaultVariables.SortPropertyName;
        }
    }
}