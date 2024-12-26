namespace Alpha.Reports.Site.Features.WhiteLabelReport.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class DailyWinLossExcelQuery : DailyWinLossGetDataQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public DailyWinLossExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}