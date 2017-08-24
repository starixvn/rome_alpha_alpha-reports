namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using ViewModels.Consolidation;

    public class ConsolidationByDateQuery :
        ConsolidationByUsernameQuery,
        IRequest<ConsolidationReportByDateViewModel>
    {
        public ConsolidationByDateQuery(IUser viewer) : base(viewer)
        {
        }
    }
}