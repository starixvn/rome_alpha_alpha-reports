namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models;
    using ViewModels.WinLossDetail;

    public class AgentWinLossDetailQuery : WinLossDetailQuery, IRequest<AgentWinLossDetailViewModel>
    {
        public override UserLevel UserLevel => UserLevel.Master;
    }
}