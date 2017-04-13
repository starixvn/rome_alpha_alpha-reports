namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class MasterOrAgentPlayForFunExcelQuery : PlayForFunQuery, IRequest<ExcelFileViewModel>
    {
        public MasterOrAgentPlayForFunExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}