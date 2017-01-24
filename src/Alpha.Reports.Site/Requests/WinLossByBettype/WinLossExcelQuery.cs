namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels;
    using WinLossByBettype;

    public class WinLossByBettypeExcelQuery : WinLossByBettypeQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossByBettypeExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}