namespace Alpha.Reports.Services
{
    using System.Threading;
    using Fanex.RemotingRef;

    public interface CacheRefService
    {
        string GetSportName(int sportTypeId);

        string GetBetType(int betTypeId);

        string GetBetTypeName(int betTypeId);

        string GetLeagueName(int leagueId);

        string GetLeagueName3rd(int league3rdId);

        string GetTeamName(int teamId);

        string GetTeamName3rd(int team3rdId);

        string GetProductName(int productId);

        string GetOddsTypeName(string oddsTypeId);

        string GetResourceDataByRefIdAndResourceId(string refId, int resourceId);
    }

    public class CacheRefServiceImpl : CacheRefService
    {
        private readonly IRemotingRefService remotingRefService;

        public CacheRefServiceImpl(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService;
        }

        public string GetSportName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);

        public string GetBetType(int betTypeId)
            => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public string GetBetTypeName(int betTypeId)
            => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public string GetLeagueName(int leagueId)
            => remotingRefService.GetLeagueName(leagueId.ToString(), LanguageCode);

        public string GetLeagueName3rd(int league3rdId)
            => remotingRefService.GetLeagueName3rd(league3rdId.ToString(), LanguageCode);

        public string GetTeamName(int teamId)
            => remotingRefService.GetTeamName(teamId.ToString(), LanguageCode);

        public string GetTeamName3rd(int team3rdId)
            => remotingRefService.GetTeamName3rd(team3rdId.ToString(), LanguageCode);

        public string GetOddsTypeName(string oddsTypeId)
            => remotingRefService.GetOddsTypeName(oddsTypeId, LanguageCode);

        public string GetProductName(int productId)
            => GetResourceDataByRefIdAndResourceId("Product", productId);

        public string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                LanguageCode,
                string.Empty);

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}
