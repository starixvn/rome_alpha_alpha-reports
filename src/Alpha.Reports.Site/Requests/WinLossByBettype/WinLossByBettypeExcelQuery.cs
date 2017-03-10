namespace Alpha.Reports.Site.Requests.WinLossByBetType
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossByBetTypeExcelQuery : WinLossByBetTypeQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossByBetTypeExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}