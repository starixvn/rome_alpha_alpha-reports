namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class WinLossDetailExcelNewQuery : WinLossDetailGetDataWeekQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public WinLossDetailExcelNewQuery(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }
    }
}