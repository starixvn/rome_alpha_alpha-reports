namespace Alpha.Reports.Site.Requests.MatchWinLossDetail
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class MatchWinLossDetailExcelQuery : MatchWinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        public MatchWinLossDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}