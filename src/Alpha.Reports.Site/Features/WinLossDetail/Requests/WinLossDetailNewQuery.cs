namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailNewQuery : WinLossDetailGetDataQuery, IAsyncRequest<WinLossDetailNewViewModel>
    {
        public WinLossDetailNewQuery(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
        }

        public int SubAccId { get; }
    }
}