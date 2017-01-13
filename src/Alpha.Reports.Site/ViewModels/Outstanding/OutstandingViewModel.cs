namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using App_GlobalResources;
    using Enumerations;
    using Reports.Models;
    using Reports.Models.Outstanding;

    public class OutstandingViewModel
    {
        // Always need to show member outstanding
        private readonly List<UserLevel> displayOutstandingLevels = new List<UserLevel>() { UserLevel.Member };

        public OutstandingViewModel(
            UserLevel viewerLevel,
            UserLevel reportLevel,
            IEnumerable<OutstandingData> outstandings,
            string breadcrumbLevelsJson,
            IEnumerable<CustInfo> breadcrumbLevels)
        {
            ViewerLevel = viewerLevel;
            ReportLevel = reportLevel;
            OutstandingDatasource = outstandings;
            BreadcrumbLevelsJson = breadcrumbLevelsJson;
            BreadcrumbLevels = breadcrumbLevels;

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

        public IEnumerable<OutstandingData> OutstandingDatasource { get; }

        public OutstandingData Total => OutstandingData.GetAccumulatedData(OutstandingDatasource);

        public List<UserLevel> DisplayOutstandingLevels => displayOutstandingLevels;

        public string BreadcrumbLevelsJson { get; }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; }

        public int NumberOfOutstandingColumn => DisplayOutstandingLevels.Count;
    }
}