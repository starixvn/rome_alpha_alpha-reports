namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using ViewModels.Consolidation;

    public class ConsolidationByUsernameQuery :
        GetConsolidationQuery,
        IRequest<ConsolidationReportByUsernameViewModel>
    {
        public ConsolidationByUsernameQuery(IUser viewer) : base(viewer)
        {
        }
    }
}