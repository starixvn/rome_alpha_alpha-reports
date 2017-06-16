namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossDetail;
    using WinLoss;

    public class WinLossDetailQuery :
        WinLossQuery,
        IRequest<WinLossDetailViewModel>
    {
        public WinLossDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}