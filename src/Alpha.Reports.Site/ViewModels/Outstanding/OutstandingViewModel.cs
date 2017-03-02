namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Constants;
    using Enumerations;
    using Extensions;
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
               : base(query.CustLevel, breadcrumbLevel, "~/_Reports/BetList/OutstandingDetail.aspx")
        {
            Query = query;
            OutstandingDatasource = outstandingDataSource;
            ProductMultiSelectionViewModel = new ProductMultiSelectionViewModel(products, selectedProductIds.JoinWithComma(), string.Empty);

            for (var level = (byte)(ReportLevel.Value + 1); level <= Query.ViewerLevel.Value; level++)
            {
                displayOutstandingLevels.Add(Enumeration.FromValue<UserLevel>(level));
            }
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

        public string SortPropertyName
            => string.IsNullOrWhiteSpace(Query.SortPropertyName)
            ? DefaultVariables.SortPropertyName
            : Query.SortPropertyName;

        public bool IsDescendingSort => Query.IsDescendingSort;

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
        };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(OutstandingModel model) => new Dictionary<string, object>
        {
            { "CustId", model.CustId },
            { "Username", model.CustName },
            { "Chkgetall", "on" },
            { "Type", "RunByDate_Mem" },
            { "ProductIds", ProductMultiSelectionViewModel.UserSelectedProductIds },
            { "FilterPostback", 1 },
        };
    }
}