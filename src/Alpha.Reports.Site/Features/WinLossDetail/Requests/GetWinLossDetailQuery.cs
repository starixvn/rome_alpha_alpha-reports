namespace Alpha.Reports.Site.Features.WinLossDetail.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class GetWinLossDetailQuery : NewWinLossDetailQuery, IAsyncRequest<WinLossDetailViewModel>
    {
        public GetWinLossDetailQuery(IUser viewer) : base(viewer)
        {
        }
    }
}