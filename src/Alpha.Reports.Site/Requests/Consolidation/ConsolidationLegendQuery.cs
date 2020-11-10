namespace Alpha.Reports.Site.Requests.Consolidation
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using MediatR;

    public class ConsolidationLegendQuery : ReportDefaultDateRangeQuery, IRequest<IEnumerable<string>>
    {
        public ConsolidationLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}