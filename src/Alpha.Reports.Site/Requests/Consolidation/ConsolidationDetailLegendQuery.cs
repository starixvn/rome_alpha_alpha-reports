namespace Alpha.Reports.Site.Requests.Consolidation
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using MediatR;

    public class ConsolidationDetailLegendQuery : ConsolidationLegendQuery, IRequest<IEnumerable<string>>
    {
        public ConsolidationDetailLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}