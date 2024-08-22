namespace Alpha.Reports.Site.Features.WhiteLabelReport.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class SettlementExcelQuery : SettlementQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public SettlementExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}