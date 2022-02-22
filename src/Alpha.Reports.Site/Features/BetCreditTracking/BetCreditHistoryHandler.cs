namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class BetCreditHistoryHandler : 
        BetCreditHistoryBaseHandler, 
        IAsyncRequestHandler<BetCreditHistoryQuery, BetCreditHistoryViewModel>
    {
        public BetCreditHistoryHandler(BetCreditHistoryService reportService) : base(reportService)
        {
        }

        public async Task<BetCreditHistoryViewModel> Handle(BetCreditHistoryQuery message)
        {
            var statementMovementParam = CreateParameter(message);
            var report = await reportService.GetBetCreditHistoryAsync(statementMovementParam);
            return new BetCreditHistoryViewModel(report, message);
        }
    }
}