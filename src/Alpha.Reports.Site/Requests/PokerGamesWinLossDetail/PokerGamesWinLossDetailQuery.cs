namespace Alpha.Reports.Site.Requests.PokerGamesWinLossDetail
{
    using MediatR;
    using Reports.Models;
    using ViewModels.PokerGamesWinLossDetail;
    using WinLoss;

    public class PokerGamesWinLossDetailQuery :
        WinLossQuery,
        IRequest<PokerGamesWinLossDetailViewModel>
    {
        public PokerGamesWinLossDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}