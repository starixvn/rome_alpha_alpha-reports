namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class MoveStatementHistoryQuery : FromLast3MonthDateRangeQuery, IRequest<MoveStatementHistoryViewModel>
    {
        public MoveStatementHistoryQuery(IUser viewer) : base(viewer)
        {
            IsProtectedMode = viewer.IsProtectedMode;
        }

        public string MemberUsername { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsProtectedMode { get; }
    }
}