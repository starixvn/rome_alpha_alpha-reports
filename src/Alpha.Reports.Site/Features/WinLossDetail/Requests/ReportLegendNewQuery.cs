namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Components;
    using MediatR;

    public class ReportLegendNewQuery : ReportBaseQuery, IAsyncRequest<ReportLegendViewModel>
    {
        public ReportLegendNewQuery(IUser viewer) : base(viewer)
        {
        }
    }
}