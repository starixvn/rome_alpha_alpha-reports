namespace Alpha.Reports.Site.Requests.Outstanding
{
    using System.Collections.Generic;
    using MediatR;

    public class ReportLegendQuery : IRequest<IEnumerable<string>>
    {
        public int CustId { get; set; }
    }
}