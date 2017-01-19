namespace Alpha.Reports.Site.Handlers.RacingCommission
{
    using MediatR;
    using Requests.WinLoss;
    using ViewModels.RacingCommission;

    public class RacingCommissionHandler : BaseHandler, IRequestHandler<RacingCommissionQuery, RacingCommissionViewModel>
    {
        public RacingCommissionViewModel Handle(RacingCommissionQuery message)
        {
            return new RacingCommissionViewModel();
        }
    }
}