namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using ViewModels.WinLossDetail;

    public class AgentWinLossDetailQuery : WinLossDetailQuery, IRequest<AgentWinLossDetailViewModel>
    {
    }
}