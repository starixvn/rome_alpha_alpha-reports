namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using Alpha.Reports.Utils;
    using MediatR;
    using System.Linq;

    public class BetChoiceMatchingHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingQuery, BetChoiceMatchingViewModel>
    {
        private readonly ReportDateService reportDateService;

        public BetChoiceMatchingHandler(ReportDateService reportDateService)
        {
            this.reportDateService = reportDateService;
        }

        public BetChoiceMatchingViewModel Handle(BetChoiceMatchingQuery message)
        {
            var betCounts = BetChoiceMatchingConstants.DefaultBetCounts;
            var betCount = betCounts.First();
            var balanceUpDay = reportDateService.GetBalanceUpDay();
            return new BetChoiceMatchingViewModel(SystemTime.Today, balanceUpDay, betCounts, betCount);
        }
    }
}