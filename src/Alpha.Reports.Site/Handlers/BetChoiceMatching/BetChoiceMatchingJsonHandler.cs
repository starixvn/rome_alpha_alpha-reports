namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;

    public class BetChoiceMatchingJsonHandler :
        BaseHandler,
        IRequestHandler<BetChoiceMatchingJsonQuery, BetChoiceMatchingJsonModel>
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly CacheRefService cacheRefService;
        private readonly LeagueService leagueService;

        public BetChoiceMatchingJsonHandler(
            BetChoiceMatchingService betChoiceMatchingService, 
            CacheRefService cacheRefService,
            LeagueService leagueService)
        {
            this.betChoiceMatchingService = betChoiceMatchingService;
            this.cacheRefService = cacheRefService;
            this.leagueService = leagueService;
        }

        public BetChoiceMatchingJsonModel Handle(BetChoiceMatchingJsonQuery message)
        {
            var queryParam = new BetChoiceMatchingParam(message.CustId, message.WinlostDate, message.MoveBetsDate, message.BetCount);
            var data = betChoiceMatchingService.Query(queryParam);
            var eventModels = data.Item1;
            var ipModels = data.Item2;
            var teams = eventModels.Select(x => x.HomeId).Union(eventModels.Select(x => x.AwayId)).Select(teamId => new KeyValuePair<int, string>(teamId, cacheRefService.GetTeamName(teamId)));
            var leagues = eventModels
                .Select(x => new { x.LeagueId, x.LeagueGroupId })
                .Distinct()
                .Select(a => new KeyValuePair<int, string>(a.LeagueId, leagueService.GetLeagueName(a.LeagueId, (byte)LeagueType.Self, a.LeagueGroupId)));
            var sportTypes = eventModels.Select(x => x.SportType).Distinct().Select(sportType => new KeyValuePair<int, string>(sportType, cacheRefService.GetSportName(sportType)));
            var betTypes = eventModels.Select(x => x.BetType).Distinct().Select(betType => new KeyValuePair<int, string>(betType, cacheRefService.GetBetTypeName(betType)));
            return new BetChoiceMatchingJsonModel(eventModels, teams, leagues, sportTypes, betTypes, ipModels);
        }
    }
}