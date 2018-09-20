namespace Alpha.Reports.Site.Handlers.PokerGamesWinLossDetail
{
    using Params;
    using Reports.Models;
    using Requests.PokerGamesWinLossDetail;
    using Services;

    public class PokerGamesWinLossDetailBaseHandler : BaseHandler
    {
        protected readonly PokerGamesWinLossDetailService reportService;

        public PokerGamesWinLossDetailBaseHandler(PokerGamesWinLossDetailService reportService)
        {
            this.reportService = reportService;
        }

        protected PokerGamesWinLossDetailParam CreateParameter(PokerGamesWinLossDetailQuery message, UserLevel userLevel)
            => new PokerGamesWinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
                 message.FromDate,
                 message.ToDate,
                 message.LimitedDateToShowInfo,
                 userLevel,
                 message.SortPropertyName,
                 message.IsDescendingSort,
                 message.IsHistoricReport);
    }
}