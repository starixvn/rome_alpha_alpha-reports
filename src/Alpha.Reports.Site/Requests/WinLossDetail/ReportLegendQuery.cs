namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using MediatR;
    using Reports.Models;

    public class ReportLegendQuery : IRequest<IEnumerable<string>>
    {
        public ReportLegendQuery()
        {
        }

        public ReportLegendQuery(IUser viewer)
        {
            CustId = viewer.Id;
        }

        public int CustId { get; }
    }
}