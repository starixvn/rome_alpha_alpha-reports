namespace Alpha.Reports.Site.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class ReportLegendGrossCommQuery : ReportBaseQuery, IRequest<ReportLegendViewModel>
    {
        public ReportLegendGrossCommQuery(IUser viewer) : base(viewer)
        {
        }
    }
}