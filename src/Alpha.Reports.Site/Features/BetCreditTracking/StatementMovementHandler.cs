namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class StatementMovementHandler : 
        StatementMovementBaseHandler, 
        IAsyncRequestHandler<StatementMovementQuery, StatementMovementViewModel>
    {
        public StatementMovementHandler(StatementMovementService reportService) : base(reportService)
        {
        }

        public async Task<StatementMovementViewModel> Handle(StatementMovementQuery message)
        {
            var statementMovementParam = CreateParameter(message);
            var report = await reportService.GetStatementMovementAsync(statementMovementParam);
            return new StatementMovementViewModel(report, message);
        }
    }
}