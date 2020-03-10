namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using MediatR;

    public class BetChoiceMatchingQuery : ReportBaseQuery, IRequest<BetChoiceMatchingViewModel>
    {
        public BetChoiceMatchingQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid() => base.IsValid() && ViewerLevel.IsSuperLevel();
    }
}