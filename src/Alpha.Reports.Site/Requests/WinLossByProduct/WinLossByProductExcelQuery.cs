namespace Alpha.Reports.Site.Requests.WinLossByProduct
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossByProductExcelQuery : WinLossByProductQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossByProductExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}