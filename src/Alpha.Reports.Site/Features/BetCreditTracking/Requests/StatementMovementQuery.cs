namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class StatementMovementQuery : FromLast3MonthDateRangeQuery, IAsyncRequest<StatementMovementViewModel>
    {
        public StatementMovementQuery(IUser viewer) : base(viewer)
        {
        }
    }
}