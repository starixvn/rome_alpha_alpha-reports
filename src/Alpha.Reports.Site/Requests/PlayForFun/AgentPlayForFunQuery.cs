namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels.PlayForFun;

    public class AgentPlayForFunQuery : PlayForFunQuery, IRequest<AgentPlayForFunReportViewModel>
    {
        public AgentPlayForFunQuery(IUser viewer) : base(viewer)
        {
            CustLevelId = UserLevel.Master.Value;
        }
    }
}