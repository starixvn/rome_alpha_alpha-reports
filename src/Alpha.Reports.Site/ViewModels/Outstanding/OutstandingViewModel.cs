namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Enumerations;
    using Extensions;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Requests.Outstanding;

    public class OutstandingViewModel : DrillDownableReportViewModel<OutstandingModel>
    {
        // Always need to show member outstanding
        private readonly List<UserLevel> displayOutstandingLevels = new List<UserLevel>() { UserLevel.Member };

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
            { "UserSelectedProductIds", ProductMultiSelectionViewModel.UserSelectedProductIds }
        };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(OutstandingModel model) => new Dictionary<string, object>
        {
            { "CustId", model.CustId },
            { "Username", model.CustName },
            { "Chkgetall", "on" },
            { "Type", "RunByDate_Mem" },
            { "ProductIds", Query.UserSelectedProductIds },
            { "FilterPostback", 1 },
        };
    }
}