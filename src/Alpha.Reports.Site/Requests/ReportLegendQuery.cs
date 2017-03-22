namespace Alpha.Reports.Site.Requests
{
    using System.Collections.Generic;
    using MediatR;

    public class ReportLegendQuery : IRequest<IEnumerable<string>>
    {
    }
}