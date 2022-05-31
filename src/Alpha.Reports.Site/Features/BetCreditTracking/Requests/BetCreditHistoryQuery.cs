namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class BetCreditHistoryQuery : FromBalanceUpDateDateRangeQuery, IRequest<BetCreditHistoryViewModel>
    {
        public BetCreditHistoryQuery(IUser viewer) : base(viewer)
        {
        }

        public string Member { get; set; }

        public long? RefNo { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;
    }
}