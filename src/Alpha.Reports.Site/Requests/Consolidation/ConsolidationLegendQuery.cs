namespace Alpha.Reports.Site.Requests.Consolidation
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class ConsolidationLegendQuery : ReportDefaultDateRangeQuery, IRequest<ReportLegendViewModel>
    {
        public ConsolidationLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}