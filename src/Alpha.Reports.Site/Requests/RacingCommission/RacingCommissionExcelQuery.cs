namespace Alpha.Reports.Site.Requests.RacingCommission
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class RacingCommissionExcelQuery : RacingCommissionQuery, IRequest<ExcelFileViewModel>
    {
        public RacingCommissionExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}