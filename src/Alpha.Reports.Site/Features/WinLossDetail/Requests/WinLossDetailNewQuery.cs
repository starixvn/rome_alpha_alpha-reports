namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Web;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailNewQuery : WinLossDetailGetDataQuery, IAsyncRequest<WinLossDetailNewViewModel>
    {
        public WinLossDetailNewQuery(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
            IsInternal = viewer.IsInternal;
        }

        public int SubAccId { get; }

        public bool IsInternal { get; }
    }
}