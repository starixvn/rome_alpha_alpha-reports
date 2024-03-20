namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailGetDataQuery : WinLossDetailGetDataWeekQuery, IAsyncRequest<WinLossDetailGetDataViewModel>
    {
        public WinLossDetailGetDataQuery(IUser viewer) : base(viewer)
        {
        }
    }
}