namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Enumerations;
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