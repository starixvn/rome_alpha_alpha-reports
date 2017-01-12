namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Constants;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossDetailExcelQuery : WinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossDetailExcelQuery(IUser user) : base(user)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }

        public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);
    }
}