namespace Alpha.Reports.Site.Requests.MatchWinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class MatchWinLossExcelQuery : FromBalanceUpDateDateRangeQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}