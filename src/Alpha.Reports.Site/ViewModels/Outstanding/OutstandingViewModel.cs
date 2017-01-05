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
                if (CustLevel.IsSuperLevel())
                {
                    return "Master Outstanding";
                }

                if (CustLevel.IsMasterLevel())
                {
                    return "Agent Outstanding";
                }

                return "Member Outstanding";
            }
        }

        //public IEnumerable<ActionLinkViewModel> BreadcrumbLevels { get; set; }

        public UserLevel CustLevel { get; set; }

        public IEnumerable<OutstandingModel> Outstandings { get; set; }

        public OutstandingModel Total { get; set; }

    }
}