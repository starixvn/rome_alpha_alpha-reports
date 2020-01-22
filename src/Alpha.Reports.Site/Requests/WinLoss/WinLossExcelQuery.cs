namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}