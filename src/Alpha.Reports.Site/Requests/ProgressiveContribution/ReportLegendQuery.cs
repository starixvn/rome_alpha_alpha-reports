namespace Alpha.Reports.Site.Requests.ProgressiveContribution
{
    using System.Collections.Generic;
    using MediatR;
    using Reports.Models;

    public class ReportLegendQuery :
        ReportBaseQuery,
        IRequest<IEnumerable<string>>
    {
        public ReportLegendQuery(IUser viewer)
            : base(viewer)
        {
            CustId = viewer.Id;
            EnableJackpotProductIds = viewer.EnableJackpotProductIds;
        }

        public IList<int> EnableJackpotProductIds { get; }
    }
}
