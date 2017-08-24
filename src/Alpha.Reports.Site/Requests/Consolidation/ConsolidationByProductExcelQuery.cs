namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class ConsolidationByProductExcelQuery : GetConsolidationQuery, IRequest<ExcelFileViewModel>
    {
        public ConsolidationByProductExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}