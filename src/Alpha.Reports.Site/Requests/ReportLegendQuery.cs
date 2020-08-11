namespace Alpha.Reports.Site.Requests
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using MediatR;

    public class ReportLegendQuery : ReportBaseQuery, IRequest<IEnumerable<string>>
    {
        public ReportLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}