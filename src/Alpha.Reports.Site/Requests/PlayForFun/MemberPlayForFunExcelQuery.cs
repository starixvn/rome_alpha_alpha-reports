namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class MemberPlayForFunExcelQuery : PlayForFunQuery, IRequest<ExcelFileViewModel>
    {
        public MemberPlayForFunExcelQuery(IUser viewer) : base(viewer)
        {
            CustLevelId = UserLevel.Agent.Value;
        }
    }
}