namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using ViewModels.Consolidation;

    public class ConsolidationByProductQuery :
        ConsolidationByUsernameQuery,
        IRequest<ConsolidationReportByProductViewModel>
    {
        public ConsolidationByProductQuery(IUser viewer) : base(viewer)
        {
        }
    }
}