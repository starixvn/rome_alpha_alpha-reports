namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.Outstanding;

    public class OutstandingViewModel
    {
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

        public UserLevel CustLevel { get; set; }

        public IEnumerable<OutstandingModel> Outstandings { get; set; }

        public OutstandingModel Total { get; set; }
    }
}