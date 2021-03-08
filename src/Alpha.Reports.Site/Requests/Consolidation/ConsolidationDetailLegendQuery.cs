namespace Alpha.Reports.Site.Requests.Consolidation
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class ConsolidationDetailLegendQuery : ConsolidationLegendQuery, IRequest<ReportLegendViewModel>
    {
        public ConsolidationDetailLegendQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
           => ViewerId > 0 && !string.IsNullOrWhiteSpace(Language);
    }
}