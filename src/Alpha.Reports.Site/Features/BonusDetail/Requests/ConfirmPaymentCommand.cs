namespace Alpha.Reports.Site.Features.BonusDetail.Requests
{
    using Alpha.Reports.Models;
    using MediatR;

    public class ConfirmPaymentCommand : ConfirmPaymentQuery, IAsyncRequest<Result>
    {
        public ConfirmPaymentCommand(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
            IsInternal = viewer.IsInternal;
            LanguageKey = viewer.LanguageKey;
        }

        public int SubAccId { get; }

        public string IP { get; }

        public bool IsInternal { get; }

        public string LanguageKey { get; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}