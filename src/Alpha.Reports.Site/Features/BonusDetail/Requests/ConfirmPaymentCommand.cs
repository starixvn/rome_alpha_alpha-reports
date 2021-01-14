namespace Alpha.Reports.Site.Features.BonusDetail.Requests
{
    using System;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class ConfirmPaymentCommand : ReportBaseQuery, IAsyncRequest<Result>
    {
        public ConfirmPaymentCommand(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
            IsInternal = viewer.IsInternal;
            LanguageKey = viewer.LanguageKey;
        }

        public long TransId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int SubAccId { get; }

        public string IP { get; }

        public bool IsInternal { get; }

        public string LanguageKey { get; }

        public override bool IsValid()
            => base.IsValid() && TransId > 0 && ViewerLevel.IsAgentLevel() && !IsInternal;
    }
}