namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailGetDataQuery : WinLossDetailIndexQuery, IAsyncRequest<WinLossDetailNewViewModel>
    {
        public WinLossDetailGetDataQuery(IUser viewer) : base(viewer)
        {
        }

        public string ProductIds { get; set; }

        public string ResourceIds { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}