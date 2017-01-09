namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Models;
    using Reports.Models.Outstanding;

    public class OutstandingViewModel
    {
        public OutstandingViewModel(UserLevel custLevel, IEnumerable<OutstandingModel> outstandings)
        {
            CustLevel = custLevel;
            Outstandings = outstandings;

            Total = new OutstandingModel(
                "Total",
                0,
                outstandings.Sum(n => n.MemberOutstanding),
                outstandings.Sum(n => n.AgentOutstanding),
                outstandings.Sum(n => n.MasterOutstanding),
                outstandings.Sum(n => n.SuperOutstanding));
        }

        public string Title
        {
            get
            {
                if (CustLevel.IsMasterLevel())
                {
                    return "Master Outstanding";
                }

                if (CustLevel.IsAgentLevel())
                {
                    return "Agent Outstanding";
                }

                return "Member Outstanding";
            }
        }

        public UserLevel CustLevel { get; }

        public IEnumerable<OutstandingModel> Outstandings { get; }

        public OutstandingModel Total { get; }
    }
}