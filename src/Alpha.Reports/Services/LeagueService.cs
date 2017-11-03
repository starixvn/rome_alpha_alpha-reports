namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using RemotingRef;

    public interface LeagueService
    {
        IEnumerable<League> GetLeaguesByCustomer(GetLeaguesByCustomerParam getLeaguesByCustomerParam);

        Dictionary<int, string> GetLeagueNames(IEnumerable<int> leagueIds);
    }

    public class LeagueServiceImpl : LeagueService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;

        public LeagueServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<League> GetLeaguesByCustomer(GetLeaguesByCustomerParam getLeaguesByCustomerParam)
        {
            var data = dynamicRepository.Fetch<League>(
                new GetLeaguesByCustomerCriteria(getLeaguesByCustomerParam, reportDateService.GetMoveBetsDay()));

            return data.Select(league => new League(league.LeagueId, league.SportTypeId, league.Name, league.LeagueType));
        }

        public Dictionary<int, string> GetLeagueNames(IEnumerable<int> leagueIds)
        {
            var leagueNames = new Dictionary<int, string>();

            if (leagueIds == null)
            {
                return leagueNames;
            }

            var distinctLeagueIds = leagueIds.Distinct();

            foreach (var leagueId in distinctLeagueIds)
            {
                var name = cacheRefService.GetLeagueName(leagueId);
                name = string.IsNullOrEmpty(name) ? cacheRefService.GetLeagueName3rd(leagueId) : name;
                leagueNames.Add(leagueId, name);
            }

            return leagueNames;
        }
    }
}
