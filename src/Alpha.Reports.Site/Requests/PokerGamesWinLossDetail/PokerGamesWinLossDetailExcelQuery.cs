namespace Alpha.Reports.Site.Requests.PokerGamesWinLossDetail
{
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using ViewModels;

    public class PokerGamesWinLossDetailExcelQuery : PokerGamesWinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        public PokerGamesWinLossDetailExcelQuery(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }

        public PagedReportData<PokerGamesWinLossDetailModel> ReportData { get; set; }
    }
}