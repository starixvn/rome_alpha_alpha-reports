namespace Alpha.Reports.Site.Requests.Consolidation
{
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using Reports.Models;
    using Reports.Models.WinLossDetail;

    public class ConsolidationDetailExcelQuery : ConsolidationDetailQuery, IRequest<ExcelFileViewModel>
    {
        public ConsolidationDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}