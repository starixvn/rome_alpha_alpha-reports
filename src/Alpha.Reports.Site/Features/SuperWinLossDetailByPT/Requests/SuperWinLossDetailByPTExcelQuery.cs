namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT.Requests
{
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using Reports.Models;

    public class SuperWinLossDetailByPTExcelQuery
        : SuperWinLossDetailByPTQuery, IRequest<ExcelFileViewModel>
    {
        public SuperWinLossDetailByPTExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}