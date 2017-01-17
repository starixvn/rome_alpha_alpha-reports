namespace Alpha.Reports.Site.Requests.MatchWinLossDetail
{
    using MediatR;
    using Reports.Models;
    using ViewModels.MatchWinLossDetail;

    public class MatchWinLossDetailQuery :
        FromBalanceUpDateDateRangeQuery,
        IRequest<MatchWinLossDetailReportViewModel>
    {
        public MatchWinLossDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public string UserSelectedLeagueIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}