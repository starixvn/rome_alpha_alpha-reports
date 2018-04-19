namespace Alpha.Reports.Site.Requests.ProgressiveContribution
{
    using System.Collections.Generic;
    using MediatR;
    using Reports.Models;
    using ViewModels.ProgressiveContribution;

    public class ProgressiveContributionQuery :
        ReportDefaultDateRangeQuery,
        IRequest<ProgressiveContributionViewModel>
    {
        public ProgressiveContributionQuery(IUser viewer) : base(viewer)
        {
            EnableJackpotProductIds = viewer.EnableJackpotProductIds;
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

        public IList<int> EnableJackpotProductIds { get; }
    }
}