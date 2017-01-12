namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossExcelQuery(IUser user) : base(user)
        {
        }

        public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);
    }
}