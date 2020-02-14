namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using MediatR;
    using System.Linq;

    public class BetChoiceMatchingJsonHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingJsonQuery, BetChoiceMatchingJsonModel>
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly CacheRefService cacheRefService;

        public BetChoiceMatchingJsonHandler(BetChoiceMatchingService betChoiceMatchingService, CacheRefService cacheRefService)
        {
            this.betChoiceMatchingService = betChoiceMatchingService;
            this.cacheRefService = cacheRefService;
        }

        public BetChoiceMatchingJsonModel Handle(BetChoiceMatchingJsonQuery message)
        {
            var queryParam = new BetChoiceMatchingParam(message.CustId, message.WinlostDate, message.MoveBetsDate, message.BetCount);
            var data = betChoiceMatchingService.Query(queryParam);
            var eventModels = data.Item1;
            var ipModels = data.Item2;
            var teams = eventModels.Select(x => x.HomeId).Union(eventModels.Select(x => x.AwayId)).Select(teamId => new KeyValueModel(teamId, cacheRefService.GetTeamName(teamId)));
            var leagues = eventModels.Select(x => x.LeagueId).Distinct().Select(leagueId => new KeyValueModel(leagueId, cacheRefService.GetLeagueName(leagueId)));
            var sportTypes = eventModels.Select(x => x.SportType).Distinct().Select(sportType => new KeyValueModel(sportType, cacheRefService.GetSportName(sportType)));
            var betTypes = eventModels.Select(x => x.BetType).Distinct().Select(betType => new KeyValueModel(betType, cacheRefService.GetBetTypeName(betType)));
            return new BetChoiceMatchingJsonModel(eventModels, teams, leagues, sportTypes, betTypes, ipModels);
        }
    }
}