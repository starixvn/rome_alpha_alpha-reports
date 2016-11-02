namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using ViewModels.WinLossDetail;

    public class AgentWinLossDetailQuery : 
        WinLossDetailQuery, 
        IRequest<WinLossDetailViewModel<AgentWinLossDetail>>
    {
        public override UserLevel UserLevel => UserLevel.Master;
    }
}