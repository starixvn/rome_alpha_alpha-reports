namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class ReportLegendQuery : ReportBaseQuery, IRequest<ReportLegendViewModel>
    {
        public ReportLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}