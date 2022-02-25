namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class MoveStatementHistoryHandler : 
        MoveStatementHistoryBaseHandler, 
        IAsyncRequestHandler<MoveStatementHistoryQuery, MoveStatementHistoryViewModel>
    {
        public MoveStatementHistoryHandler(MoveStatementHistoryService reportService) : base(reportService)
        {
        }

        public async Task<MoveStatementHistoryViewModel> Handle(MoveStatementHistoryQuery message)
        {
            var moveStatementHistoryParam = CreateParameter(message);
            var report = await reportService.GetMoveStatementHistoryAsync(moveStatementHistoryParam);
            return new MoveStatementHistoryViewModel(report, message);
        }
    }
}