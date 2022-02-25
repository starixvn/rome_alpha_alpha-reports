namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class MoveStatementHistoryQuery : FromLast3MonthDateRangeQuery, IAsyncRequest<MoveStatementHistoryViewModel>
    {
        public MoveStatementHistoryQuery(IUser viewer) : base(viewer)
        {
        }

        public string MemberUsername { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }
    }
}