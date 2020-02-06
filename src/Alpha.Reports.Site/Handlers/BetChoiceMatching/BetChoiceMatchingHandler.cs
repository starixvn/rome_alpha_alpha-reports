namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using MediatR;
    using System.Linq;

    public class BetChoiceMatchingHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingQuery, BetChoiceMatchingQueryResult>
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly CacheRefService cacheRefService;

        public BetChoiceMatchingHandler(BetChoiceMatchingService betChoiceMatchingService, CacheRefService cacheRefService)
        {
            this.betChoiceMatchingService = betChoiceMatchingService;
            this.cacheRefService = cacheRefService;
        }

        public BetChoiceMatchingQueryResult Handle(BetChoiceMatchingQuery message)
        {
            var queryParam = new BetChoiceMatchingParam(message.CustId, message.WinlostDate, message.MoveBetsDate, message.BetCount);
            var items = betChoiceMatchingService.Query(queryParam);
            var teams = items.Select(x => x.HomeId).Union(items.Select(x => x.AwayId)).Select(teamId => new KeyValueModel(teamId, cacheRefService.GetTeamName(teamId)));
            var leagues = items.Select(x => x.LeagueId).Distinct().Select(leagueId => new KeyValueModel(leagueId, cacheRefService.GetLeagueName(leagueId)));
            var sportTypes = items.Select(x => x.SportType).Distinct().Select(sportType => new KeyValueModel(sportType, cacheRefService.GetSportName(sportType)));
            var betTypes = items.Select(x => x.BetType).Distinct().Select(betType => new KeyValueModel(betType, cacheRefService.GetBetTypeName(betType)));
            return new BetChoiceMatchingQueryResult(items, teams, leagues, sportTypes, betTypes);
        }
    }
}