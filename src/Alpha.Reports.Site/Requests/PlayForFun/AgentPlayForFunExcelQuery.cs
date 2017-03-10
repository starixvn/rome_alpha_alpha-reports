namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class AgentPlayForFunExcelQuery : PlayForFunQuery, IRequest<ExcelFileViewModel>
    {
        public AgentPlayForFunExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}