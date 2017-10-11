namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Extensions;
    using Reports.Constants;
    using Reports.Helpers;
    using Reports.Models.MatchWinLossDetail;

    public class MatchWinLossDetailViewModel
    {
        public MatchWinLossDetailViewModel(
            int leagueId,
            string leagueName,
            int matchId,
            int race,
            IEnumerable<BetTypeWinLossViewModel> betTypeWinLoss,
            BetTypeWinLossViewModel total,
            MatchWinLossDetailModel firstWinLoss,
            Dictionary<int, string> teamNames)
        {
            MatchId = matchId;
            LeagueId = leagueId;
            Race = race;
            BetTypeWinLoss = betTypeWinLoss;
            Total = total;

            MatchName = GetMatchTitle(firstWinLoss, teamNames);
            LeagueName = GetLeagueTitle(firstWinLoss, leagueName);
        }

        public int LeagueId { get; }

        public string LeagueName { get; }

        public int MatchId { get; }

        public string MatchName { get; }

        public int Race { get; }

        public IEnumerable<BetTypeWinLossViewModel> BetTypeWinLoss { get; }

        public BetTypeWinLossViewModel Total { get; }

        public bool RenderSubTotal => MatchId != 0 || Race != 0;

        private string GetMatchTitle(MatchWinLossDetailModel data, Dictionary<int, string> teamNames)
        {
            if (data == null)
            {
                return string.Empty;
            }

            if (data.BetType == BetTypeIdConstants.Outright)
            {
                return string.Empty;
            }

            if (BetTypeHelper.IsRacing(data.BetType))
            {
                return $"{Resources.Race} {data.Race}";
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return $"{Resources.NumberGameNo} {data.MatchCode}";
            }

            if (BetTypeHelper.IsLucky3(data.BetType))
            {
                return $"{Resources.Lucky3GameNo} {data.MatchCode}";
            }

            if (BetTypeHelper.IsVirtualHorseRacing(data.BetType))
            {
                DateTime eventTime;
                bool isValidShowTime = DateTime.TryParse(data.ShowTime, out eventTime);

                return isValidShowTime ? eventTime.ToLongDateWithSlash() : string.Empty;
            }

            return string.Join(
                string.Empty,
                teamNames.ContainsKey(data.HomeId) ? teamNames[data.HomeId] : string.Empty,
                data.IsNeutral ? Resources.Neutral : string.Empty,
                Resources.vs,
                teamNames.ContainsKey(data.AwayId) ? teamNames[data.AwayId] : string.Empty);
        }

        private string GetLeagueTitle(MatchWinLossDetailModel data, string leagueName)
        {
            if (data == null)
            {
                return string.Empty;
            }

            if (BetTypeHelper.IsRacing(data.BetType))
            {
                return $"{Resources.HorseRacing} - {leagueName}";
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return string.IsNullOrWhiteSpace(leagueName)
                    ? Resources.NumberGame
                    : $"{Resources.NumberGame} - {leagueName}";
            }

            if(BetTypeHelper.IsLucky3(data.BetType))
            {
                return $"{Resources.Lucky3} - {leagueName}";
            }

            return leagueName;
        }
    }
}