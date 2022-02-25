namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Handlers;

    public class MoveStatementHistoryBaseHandler : BaseHandler
    {
        protected readonly MoveStatementHistoryService reportService;

        public MoveStatementHistoryBaseHandler(MoveStatementHistoryService reportService)
        {
            this.reportService = reportService;
        }

        protected MoveStatementHistoryParam CreateParameter(MoveStatementHistoryQuery message)
            => new MoveStatementHistoryParam(message.MemberUsername, message.FromDate, message.ToDate, message.LimitedDateToShowInfo);
    }
}