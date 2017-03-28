namespace Alpha.Reports.Site.Requests.MatchWinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels.MatchWinLoss;

    public class MatchWinLossQuery :
        FromBalanceUpDateDateRangeQuery,
        IRequest<MatchWinLossReportViewModel>
    {
        public MatchWinLossQuery(IUser viewer) : base(viewer)
        {
        }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }
    }
}