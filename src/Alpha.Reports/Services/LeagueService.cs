namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Configurations;
    using Constants;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface LeagueService
    {
        IEnumerable<League> GetLeaguesByCustomer(GetLeaguesByCustomerParam getLeaguesByCustomerParam);

        string GetLeagueName(int leagueId, byte leagueType);

        string GetLeagueName(int leagueId, byte leagueType, int leagueGroupId);
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

            return data.Select(l => new League(
                l.LeagueId,
                l.SportTypeId,
                GetLeagueName(l.LeagueId, l.LeagueType, l.LeagueGroupId),
                l.LeagueType,
                l.LeagueGroupId));
        }

        public string GetLeagueName(int leagueId, byte leagueType)
            => GetLeagueName(leagueId, leagueType, 0);

        public string GetLeagueName(int leagueId, byte leagueType, int leagueGroupId)
        {
            if (leagueType == (byte)LeagueType.ThirdParty)
            {
                return cacheRefService.GetLeagueName3rd(leagueId);
            }

            return AppSettings.IsSiteLabel777 ?
                cacheRefService.GetLeagueName(leagueId, leagueGroupId) :
                cacheRefService.GetLeagueName(leagueId);
        }
    }
}
