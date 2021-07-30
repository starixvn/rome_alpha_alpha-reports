namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class ReportLegendQuery : ReportBaseQuery, IRequest<ReportLegendViewModel>
    {
        public ReportLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}