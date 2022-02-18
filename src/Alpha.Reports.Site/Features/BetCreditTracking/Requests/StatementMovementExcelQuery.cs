namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class StatementMovementExcelQuery : StatementMovementQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public StatementMovementExcelQuery(IUser viewer) : base(viewer)
        {

        }

        public ReportData<StatementMovementModel> ReportData { get; set; }
    }
}