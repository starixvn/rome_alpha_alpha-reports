namespace Alpha.Reports.Site.Features.WhiteLabelReport.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels;
    using MediatR;

    public class SettlementQuery : SettlementGetDataQuery, IAsyncRequest<SettlementViewModel>
    {
        public SettlementQuery(IUser viewer) : base(viewer)
        {
        }
    }
}