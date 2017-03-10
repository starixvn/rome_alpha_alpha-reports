namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels.PlayForFun;

    public class MasterPlayForFunQuery : PlayForFunQuery, IRequest<MasterPlayForFunReportViewModel>
    {
        public MasterPlayForFunQuery(IUser viewer) : base(viewer)
        {
            CustLevelId = UserLevel.Super.Value;
        }
    }
}