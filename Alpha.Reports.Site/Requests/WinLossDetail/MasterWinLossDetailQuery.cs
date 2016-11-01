namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models;
    using ViewModels.WinLossDetail;

    public class MasterWinLossDetailQuery : WinLossDetailQuery, IRequest<MasterWinLossDetailViewModel>
    {
        public override UserLevel UserLevel => UserLevel.Super;
    }
}