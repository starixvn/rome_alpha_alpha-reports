namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using Alpha.Reports.Utils;
    using MediatR;
    using System.Linq;

    public class BetChoiceMatchingHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingQuery, BetChoiceMatchingViewModel>
    {
        public BetChoiceMatchingViewModel Handle(BetChoiceMatchingQuery message)
        {
            var betCounts = BetChoiceMatchingConstants.DefaultBetCounts;
            var betCount = betCounts.First();
            return new BetChoiceMatchingViewModel(SystemTime.Today, betCounts, betCount);
        }
    }
}