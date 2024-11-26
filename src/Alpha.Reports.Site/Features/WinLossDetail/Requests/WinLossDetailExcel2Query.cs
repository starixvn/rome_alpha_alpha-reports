namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class WinLossDetailExcel2Query : WinLossDetailGetDataWeekQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public WinLossDetailExcel2Query(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }
    }
}