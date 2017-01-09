namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using App_GlobalResources;
    using Reports.Models;
    using Reports.Models.Outstanding;

    public class OutstandingViewModel
    {
        public OutstandingViewModel(UserLevel viewerLevel, UserLevel custLevel, IEnumerable<OutstandingData> outstandings)
        {
            ViewerLevel = viewerLevel;
            ReportLevel = custLevel;
            Outstandings = outstandings;

            Total = new OutstandingData();
            foreach (var outstanding in outstandings)
            {
                Total.Accumulate(outstanding);
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

                return Resources.Member;
            }
        }

        public UserLevel ViewerLevel { get; }

        public UserLevel ReportLevel { get; }

        public IEnumerable<OutstandingData> Outstandings { get; }

        public OutstandingData Total { get; }
    }
}