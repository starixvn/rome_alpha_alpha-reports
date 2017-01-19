namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels.RacingCommission;

    public class RacingCommissionQuery : ReportBaseQuery, IRequest<RacingCommissionViewModel>
    {
        public RacingCommissionQuery(IUser viewer) : base(viewer)
        {
        }
    }
}