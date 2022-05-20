namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Handlers;

    public class BetCreditHistoryBaseHandler : BaseHandler
    {
        protected readonly BetCreditHistoryService reportService;

        public BetCreditHistoryBaseHandler(BetCreditHistoryService reportService)
        {
            this.reportService = reportService;
        }

        protected BetCreditHistoryParam CreateParameter(BetCreditHistoryQuery message)
            => new BetCreditHistoryParam(
                message.Member, 
                message.RefNo, 
                message.FromDate, 
                message.ToDate, 
                message.ViewerId, 
                message.LimitedDateToShowInfo,
                message.Language,
                message.PageIndex,
                message.PageSize);

        protected BetCreditHistoryParam CreateExcelParameter(BetCreditHistoryQuery message)
            => new BetCreditHistoryParam(
                message.Member,
                message.RefNo,
                message.FromDate,
                message.ToDate,
                message.ViewerId,
                message.LimitedDateToShowInfo,
                message.Language,
                1,
                int.MaxValue - 1);
    }
}