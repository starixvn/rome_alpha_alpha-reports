namespace Alpha.Reports.Site.Requests.WinLossByGameType
{
    using MediatR;
    using Reports.Models;
    using ViewModels;
    using WinLossByBettype;

    public class WinLossByGameTypeExcelQuery : WinLossByGameTypeQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossByGameTypeExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}