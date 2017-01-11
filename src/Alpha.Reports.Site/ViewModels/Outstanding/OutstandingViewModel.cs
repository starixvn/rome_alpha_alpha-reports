namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Enumerations;
    using Reports.Models;
    using Reports.Models.Outstanding;

    public class OutstandingViewModel
    {
        private readonly List<UserLevel> outstandingLevels = new List<UserLevel>();

        public OutstandingViewModel(
            UserLevel viewerLevel, 
            UserLevel custLevel, 
            IEnumerable<OutstandingData> outstandings, 
            string drillDownInfoJson, 
            IEnumerable<ActionLinkViewModel> breadcrumbLevels)
        {
            ViewerLevel = viewerLevel;
            ReportLevel = custLevel;
            OutstandingDatasource = outstandings;
            DrillDownInfoJson = drillDownInfoJson;
            BreadcrumbLevels = breadcrumbLevels;

            for (var level = ReportLevel.Value; level < ViewerLevel.Value; level++)
            {
                outstandingLevels.Add(Enumeration.FromValue<UserLevel>(level));
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

        public string Level
        {
            get
            {
                if (ReportLevel.IsMasterLevel())
                {
                    return Resources.Master;
                }

                if (ReportLevel.IsAgentLevel())
                {
                    return Resources.Agent;
                }

                return Resources.Username;
            }
        }

        public UserLevel ViewerLevel { get; }

        public UserLevel ReportLevel { get; }

        public IEnumerable<OutstandingData> OutstandingDatasource { get; }

        public OutstandingData Total => OutstandingData.GetAccumulatedData(OutstandingDatasource);

        public List<UserLevel> OutstandingLevels => outstandingLevels;

        public string DrillDownInfoJson { get; }

        public IEnumerable<ActionLinkViewModel> BreadcrumbLevels { get; }

        public decimal GetOutstandingValue(OutstandingData data, UserLevel reportLevel)
        {
            if (reportLevel.IsMemberLevel())
            {
                return data.AgentOutstanding;
            }

            if (reportLevel.IsAgentLevel())
            {
                return data.MasterOutstanding;
            }

            if (reportLevel.IsMasterLevel())
            {
                return data.SuperOutstanding;
            }

            throw new ArgumentOutOfRangeException(nameof(reportLevel));
        }
    }
}