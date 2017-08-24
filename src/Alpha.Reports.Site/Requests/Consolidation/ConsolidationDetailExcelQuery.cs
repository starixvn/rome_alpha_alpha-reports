namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using Reports.Models.WinLossDetail;

    public class ConsolidationDetailExcelQuery : ConsolidationDetailQuery, IRequest<PagedReportData<WinLossDetailModel>>
    {
        public ConsolidationDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}