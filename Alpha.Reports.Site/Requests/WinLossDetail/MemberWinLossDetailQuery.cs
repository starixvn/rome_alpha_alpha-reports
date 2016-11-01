namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models;
    using ViewModels.WinLossDetail;

    public class MemberWinLossDetailQuery : WinLossDetailQuery, IRequest<MemberWinLossDetailViewModel>
    {
        public override UserLevel UserLevel => UserLevel.Agent;
    }
}