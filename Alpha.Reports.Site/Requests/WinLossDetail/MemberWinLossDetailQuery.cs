namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Models;
    using MediatR;
    using ViewModels.WinLossDetail;

    public class MemberWinLossDetailQuery : WinLossDetailQuery, IRequest<MemberWinLossDetailViewModel>
    {
        public override UserLevel UserLevel => UserLevel.Agent;
    }
}