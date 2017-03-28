namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using MediatR;

    public class ReportLegendQuery : IRequest<IEnumerable<string>>
    {
        public int CustId { get; set; }
    }
}