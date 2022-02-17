namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class BetCreditHistoryQuery : ReportBaseQuery, IAsyncRequest<BetCreditHistoryViewModel>
    {
        public BetCreditHistoryQuery(IUser viewer) : base(viewer)
        {
        }
    }
}