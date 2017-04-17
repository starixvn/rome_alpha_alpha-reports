namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels.PlayForFun;

    public class MemberPlayForFunQuery : PlayForFunQuery, IRequest<MemberPlayForFunReportViewModel>
    {
        public MemberPlayForFunQuery(IUser viewer) : base(viewer)
        {
        }
    }
}