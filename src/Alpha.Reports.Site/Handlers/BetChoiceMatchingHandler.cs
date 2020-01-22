namespace Alpha.Reports.Site.Handlers
{
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using MediatR;
    using System.Linq;

    public class BetChoiceMatchingHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingQuery, BetChoiceMatchingQueryModel>
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly CacheRefService cacheRefService;

        public BetChoiceMatchingHandler(BetChoiceMatchingService betChoiceMatchingService, CacheRefService cacheRefService)
        {
            this.betChoiceMatchingService = betChoiceMatchingService;
            this.cacheRefService = cacheRefService;
        }

        public BetChoiceMatchingQueryModel Handle(BetChoiceMatchingQuery message)
        {
            var queryParam = new BetChoiceMatchingParam(message.CustId, message.WinlostDate, message.MoveBetsDate, message.BetCount);
            var items = betChoiceMatchingService.Query(queryParam);
            var leagues = items.Select(x => x.LeagueId).Distinct().Select(leagueId => new
            {
                Id = leagueId,
                Name = cacheRefService.GetLeagueName(leagueId)
            });
            return new BetChoiceMatchingQueryModel(items);
        }
    }
}