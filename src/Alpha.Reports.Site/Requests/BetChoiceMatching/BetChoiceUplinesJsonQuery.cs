namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using MediatR;

    public class BetChoiceUplinesJsonQuery : ReportBaseQuery, IRequest<BetChoiceUplinesModel>
    {
        public BetChoiceUplinesJsonQuery(IUser viewer) : base(viewer)
        {
        }

        public int SearchAccountId { get; set; }

        public override bool IsValid() => base.IsValid() && ViewerLevel.IsSuperLevel() && SearchAccountId > 0;
    }
}