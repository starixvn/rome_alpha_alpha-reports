namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class MoveStatementHistoryExcelQuery : MoveStatementHistoryQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public MoveStatementHistoryExcelQuery(IUser viewer) : base(viewer)
        {

        }

        public ReportData<MoveStatementHistoryModel> ReportData { get; set; }
    }
}