namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Configurations;
    using Enumerations;
    using Helpers;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Web.Helpers;

    public class OutstandingViewModel
    {
        // Always need to show member outstanding
        private readonly List<UserLevel> displayOutstandingLevels = new List<UserLevel>() { UserLevel.Member };

        private string betListPath = UrlHelper.MapExMainPath("~/_Reports/BetList/OutstandingDetail.aspx");

        public OutstandingViewModel(
            UserLevel viewerLevel,
            UserLevel reportLevel,
            IEnumerable<OutstandingModel> outstandingDataSource,
            BreadcrumbLevelsViewModel breadcrumbLevelsViewModel)
        {
            ViewerLevel = viewerLevel;
            ReportLevel = reportLevel;
            OutstandingDatasource = outstandingDataSource;
            BreadcrumbLevelsViewModel = breadcrumbLevelsViewModel;

            for (var level = (byte)(ReportLevel.Value + 1); level <= ViewerLevel.Value; level++)
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

        public UserLevel ViewerLevel { get; }

        public UserLevel ReportLevel { get; }

        public IEnumerable<OutstandingModel> OutstandingDatasource { get; }

        public OutstandingModel Total => OutstandingModel.GetAccumulatedData(OutstandingDatasource);

        public List<UserLevel> DisplayOutstandingLevels => displayOutstandingLevels;

        public BreadcrumbLevelsViewModel BreadcrumbLevelsViewModel { get; }

        public int NumberOfOutstandingColumn => DisplayOutstandingLevels.Count;

        public string GetUserActionLink(OutstandingModel outstanding)
        {
            if (ReportLevel.IsMemberLevel())
            {
                return $"{betListPath}?custId={outstanding.CustId}&username={outstanding.CustName}&chkgetall=on";
            }

            return $"?custLevelId={ReportLevel.Value}&custId={outstanding.CustId}&custName={outstanding.CustName}" +
                    $"&breadcrumbLevelsJson={BreadcrumbLevelsViewModel.BreadcrumbLevelsJson}";
        }
    }
}