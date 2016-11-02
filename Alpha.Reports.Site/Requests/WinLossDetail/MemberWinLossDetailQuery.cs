namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using ViewModels.WinLossDetail;

    public class MemberWinLossDetailQuery : 
        WinLossDetailQuery, 
        IRequest<WinLossDetailViewModel<MemberWinLossDetail>>
    {
        public override UserLevel UserLevel => UserLevel.Agent;
    }
}