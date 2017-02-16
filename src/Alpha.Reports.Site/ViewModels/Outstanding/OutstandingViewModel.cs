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

    public class OutstandingViewModel
    {
        // Always need to show member outstanding
        private readonly List<UserLevel> displayOutstandingLevels = new List<UserLevel>() { UserLevel.Member };

        private string betListPath = Helpers.UrlHelper.MapExMainPath("~/_Reports/BetList/OutstandingDetail.aspx");

        public OutstandingViewModel(
            OutstandingQuery query,
            IEnumerable<OutstandingModel> outstandingDataSource,
            IEnumerable<int> selectedProductIds,
            IEnumerable<SelectListItem> products,
            BreadcrumbLevelViewModel breadcrumbLevelViewModel)
        {
            Query = query;
            OutstandingDatasource = outstandingDataSource;
            BreadcrumbLevelViewModel = breadcrumbLevelViewModel;
            ProductMultiSelectionViewModel = new ProductMultiSelectionViewModel(products, selectedProductIds.JoinWithComma(), "");

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

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel { get; }

        public int NumberOfOutstandingColumn => DisplayOutstandingLevels.Count;

        public string GetUserActionLink(OutstandingModel outstanding)
        {
            if (ReportLevel.IsMemberLevel())
            {
                return $"{betListPath}?custId={outstanding.CustId}&username={outstanding.CustName}&chkgetall=on&type=RunByDate_Mem";
            }

            return $"?custLevelId={ReportLevel.Value}&custId={outstanding.CustId}&custName={outstanding.CustName}" +
                    $"&breadcrumbLevelJson={BreadcrumbLevelViewModel.BreadcrumbLevelJson}";
        }
    }
}