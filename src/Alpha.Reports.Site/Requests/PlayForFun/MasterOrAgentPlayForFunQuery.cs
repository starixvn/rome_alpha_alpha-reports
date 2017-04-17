namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels.PlayForFun;

    public class MasterOrAgentPlayForFunQuery : PlayForFunQuery, IRequest<MasterOrAgentPlayForFunReportViewModel>
    {
        public MasterOrAgentPlayForFunQuery(IUser viewer) : base(viewer)
        {
        }
    }
}