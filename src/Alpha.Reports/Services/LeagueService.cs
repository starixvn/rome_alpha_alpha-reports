namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using RemotingRef;

    public interface LeagueService
    {
        IEnumerable<League> GetLeaguesByCustomer(GetLeaguesByCustomerParam getLeaguesByCustomerParam);
        
        string GetLeagueName(int leagueId, byte leagueType);
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

        public string GetLeagueName(int leagueId, byte leagueType)
            => leagueType == (byte)LeagueType.ThirdParty ? cacheRefService.GetLeagueName3rd(leagueId) : cacheRefService.GetLeagueName(leagueId);
    }
}
