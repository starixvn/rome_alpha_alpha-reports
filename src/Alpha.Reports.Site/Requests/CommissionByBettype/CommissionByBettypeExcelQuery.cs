namespace Alpha.Reports.Site.Requests.CommissionByBettype
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class CommissionByBettypeExcelQuery: CommissionByBettypeQuery, IRequest<ExcelFileViewModel>
    {
        public CommissionByBettypeExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}