namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class BetCreditHistoryHandler : IAsyncRequestHandler<BetCreditHistoryQuery, BetCreditHistoryViewModel>
    {
        public Task<BetCreditHistoryViewModel> Handle(BetCreditHistoryQuery message)
        {
            return Task.FromResult(new BetCreditHistoryViewModel(message.ViewerLevel.DisplayName));
        }
    }
}