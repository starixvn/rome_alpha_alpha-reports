namespace Alpha.Reports.Site.Requests.SuperWinLossDetailByPT
{
    using Alpha.Reports.Models;
    using MediatR;
    using ViewModels.Components;

    public class ReportLegendQuery : ReportBaseQuery, IRequest<ReportLegendViewModel>
    {
        public ReportLegendQuery(IUser viewer) : base(viewer)
        {
        }
    }
}