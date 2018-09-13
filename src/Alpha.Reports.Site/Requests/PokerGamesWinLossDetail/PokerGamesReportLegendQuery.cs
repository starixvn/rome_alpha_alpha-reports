namespace Alpha.Reports.Site.Requests.PokerGamesWinLossDetail
{
    using System.Collections.Generic;
    using MediatR;

    public class PokerGamesReportLegendQuery : IRequest<IEnumerable<string>>
    {
        public int CustId { get; set; }
    }
}