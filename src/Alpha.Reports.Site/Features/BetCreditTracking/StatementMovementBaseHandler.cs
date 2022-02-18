namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Params;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Handlers;

    public class StatementMovementBaseHandler : BaseHandler
    {
        protected readonly StatementMovementService reportService;

        public StatementMovementBaseHandler(StatementMovementService reportService)
        {
            this.reportService = reportService;
        }

        protected StatementMovementParam CreateParameter(StatementMovementQuery message)
            => new StatementMovementParam(message.MemberUsername, message.FromDate, message.ToDate);
    }
}