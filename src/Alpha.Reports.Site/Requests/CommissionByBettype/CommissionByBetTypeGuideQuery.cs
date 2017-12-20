namespace Alpha.Reports.Site.Requests.CommissionByBettype
{
    using System.Collections.Generic;
    using MediatR;
    using Reports.Models;

    public class CommissionByBetTypeGuideQuery : ReportBaseQuery, IRequest<IEnumerable<string>>
    {
        public CommissionByBetTypeGuideQuery(IUser viewer) : base(viewer)
        {
        }
    }
}