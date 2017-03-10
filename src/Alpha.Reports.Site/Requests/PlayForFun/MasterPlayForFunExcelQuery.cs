namespace Alpha.Reports.Site.Requests.PlayForFun
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class MasterPlayForFunExcelQuery : PlayForFunQuery, IRequest<ExcelFileViewModel>
    {
        public MasterPlayForFunExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}