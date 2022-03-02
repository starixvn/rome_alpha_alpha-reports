namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class BetCreditHistoryHandler : 
        BetCreditHistoryBaseHandler, 
        IRequestHandler<BetCreditHistoryQuery, BetCreditHistoryViewModel>
    {
        public BetCreditHistoryHandler(BetCreditHistoryService reportService) : base(reportService)
        {
        }

        public BetCreditHistoryViewModel Handle(BetCreditHistoryQuery message)
        {
            var betCreditHistoryParam = CreateParameter(message);
            var result = reportService.GetBetCreditHistory(betCreditHistoryParam);

            if (result.ErrorCode != 0)
            {
                return new BetCreditHistoryViewModel(null, message, true);
            }

            return new BetCreditHistoryViewModel(result.Data, message);
        }
    }
}