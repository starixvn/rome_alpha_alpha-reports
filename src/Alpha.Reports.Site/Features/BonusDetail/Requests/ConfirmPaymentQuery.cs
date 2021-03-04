namespace Alpha.Reports.Site.Features.BonusDetail.Requests
{
    using System;
    using System.Linq;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests;

    public class ConfirmPaymentQuery : ReportBaseQuery
    {
        public ConfirmPaymentQuery(IUser viewer) : base(viewer)
        {
        }

        public long TransId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && TransId > 0
            && ViewerLevel.IsAgentLevel()
            && SiteSettings.AllowedSitesToViewBonusDetail.Contains(Site.ToUpperInvariant());
    }
}