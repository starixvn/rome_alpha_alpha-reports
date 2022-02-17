namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class StatementMovementHandler : IAsyncRequestHandler<StatementMovementQuery, StatementMovementViewModel>
    {
        public Task<StatementMovementViewModel> Handle(StatementMovementQuery message)
        {
            return Task.FromResult(new StatementMovementViewModel(message.ViewerLevel.DisplayName));
        }
    }
}