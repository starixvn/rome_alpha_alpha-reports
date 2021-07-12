namespace Alpha.Reports.Site.Requests.SuperWinLossDetailByPT
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class SuperWinLossDetailByPTExcelQuery
        : SuperWinLossDetailByPTQuery, IRequest<ExcelFileViewModel>
    {
        public SuperWinLossDetailByPTExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}