namespace Alpha.Reports.Site.Requests.MatchWinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class MatchWinLossExcelQuery : ReportBaseQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}