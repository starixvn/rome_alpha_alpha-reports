namespace Alpha.Reports.Site.Requests.Consolidation
{
    using Reports.Models;

    public class GetConsolidationQuery : ReportDefaultDateRangeQuery
    {
        private readonly IUser viewer;

        internal GetConsolidationQuery(IUser viewer) : base(viewer)
        {
            this.viewer = viewer;
        }

        public int UserSelectedCurrencyId { get; set; }

        public string UserSelectedProductIds { get; set; }

        public string UserSelectedResourceIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

        public bool IsEnableSabaPromotion => viewer.IsEnableSabaPromotionForHeadline;
    }
}