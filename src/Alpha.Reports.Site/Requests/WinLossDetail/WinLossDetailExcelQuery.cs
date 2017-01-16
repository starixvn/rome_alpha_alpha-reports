namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Constants;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossDetailExcelQuery : WinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossDetailExcelQuery(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }
    }
}