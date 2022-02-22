namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class BetCreditHistoryExcelQuery : BetCreditHistoryQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public BetCreditHistoryExcelQuery(IUser viewer) : base(viewer)
        {

        }

        public ReportData<BetCreditHistoryModel> ReportData { get; set; }
    }
}