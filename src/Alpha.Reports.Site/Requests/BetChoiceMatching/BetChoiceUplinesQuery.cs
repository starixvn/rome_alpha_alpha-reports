namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using MediatR;

    public class BetChoiceUplinesQuery : ReportBaseQuery, IRequest<BetChoiceUplinesModel>
    {
        public BetChoiceUplinesQuery(IUser viewer) : base(viewer)
        {
        }

        public int SearchAccountId { get; set; }

        public override bool IsValid() => base.IsValid() && SearchAccountId > 0;
    }
}