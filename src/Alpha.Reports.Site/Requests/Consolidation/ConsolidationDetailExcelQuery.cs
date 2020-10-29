namespace Alpha.Reports.Site.Requests.Consolidation
{
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using Reports.Models;

    public class ConsolidationDetailExcelQuery : ConsolidationDetailQuery, IRequest<ExcelFileViewModel>
    {
        public ConsolidationDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}