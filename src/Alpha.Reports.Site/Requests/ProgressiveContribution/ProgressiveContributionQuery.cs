namespace Alpha.Reports.Site.Requests.ProgressiveContribution
{
    using MediatR;
    using Reports.Models;
    using ViewModels.ProgressiveContribution;

    public class ProgressiveContributionQuery :
        FromBalanceUpDateDateRangeQuery,
        IRequest<ProgressiveContributionViewModel>
    {
        public ProgressiveContributionQuery(IUser viewer) : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

        public string LanguageKey { get; }
    }
}