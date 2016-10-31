namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using ViewModels.WinLossDetail;

    public class MasterWinLossDetailQuery : WinLossDetailQuery, IRequest<MasterWinLossDetailViewModel>
    {
    }
}