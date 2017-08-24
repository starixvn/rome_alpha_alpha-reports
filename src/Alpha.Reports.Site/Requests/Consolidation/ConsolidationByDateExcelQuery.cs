namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class ConsolidationByDateExcelQuery : GetConsolidationQuery, IRequest<ExcelFileViewModel>
    {
        public ConsolidationByDateExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}