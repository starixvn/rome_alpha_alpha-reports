namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class EnteredSportResultExcelQuery : EnteredSportResultQuery, IRequest<ExcelFileViewModel>
    {
        public EnteredSportResultExcelQuery(IUser user) : base(user)
        {
        }
    }
}