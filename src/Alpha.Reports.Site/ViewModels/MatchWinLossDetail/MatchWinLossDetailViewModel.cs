namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Extensions;
    using Reports.Constants;
    using Reports.Helpers;
    using Reports.Models.MatchWinLossDetail;
    using Services;

    public class MatchWinLossDetailViewModel
    {
        public MatchWinLossDetailViewModel(
            int leagueId,
            string leagueName,
            int matchId,
            string race,
            IEnumerable<BetTypeWinLossViewModel> betTypeWinLoss,
            BetTypeWinLossViewModel total,
            MatchWinLossDetailModel firstWinLoss,
            string homeName,
            string awayName,
            byte leagueType)
        {
            MatchId = matchId;
            LeagueId = leagueId;
            Race = race;
            BetTypeWinLoss = betTypeWinLoss;
            Total = total;
            HomeName = homeName;
            AwayName = awayName;
            MatchName = GetMatchTitle(firstWinLoss);
            LeagueName = GetLeagueTitle(firstWinLoss, leagueName);
            LeagueType = leagueType;
        }

        public int LeagueId { get; }

        public byte LeagueType { get; }

        public string LeagueName { get; }

        public string HomeName { get; }

        public string AwayName { get; }

        public int MatchId { get; }

        public string MatchName { get; }

        public string Race { get; }

        public IEnumerable<BetTypeWinLossViewModel> BetTypeWinLoss { get; }

        public BetTypeWinLossViewModel Total { get; }

        public bool RenderSubTotal => MatchId != 0 || !string.IsNullOrEmpty(Race);

        private string GetMatchTitle(MatchWinLossDetailModel data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            if (data.BetType == BetTypeIdConstants.Outright)
            {
                return string.Empty;
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return $"{Resources.NumberGameNo} {data.MatchCode}";
            }

            if (BetTypeHelper.IsHappy5(data.BetType))
            {
                return $"{Resources.No} {data.MatchCode}";
            }

            if (BetTypeHelper.IsVirtualHorseRacing(data.BetType))
            {
                return $"{Resources.Event} {data.Race} - {Resources.ID} {data.MatchCode}";
            }

            return string.Join(
                string.Empty,
                HomeName,
                data.IsNeutral ? Resources.Neutral : string.Empty,
                Resources.vs,
                AwayName);
        }

        private string GetLeagueTitle(MatchWinLossDetailModel data, string leagueName)
        {
            if (data == null)
            {
                return string.Empty;
            }

            if (BetTypeHelper.IsNumberGame(data.BetType))
            {
                return string.IsNullOrWhiteSpace(leagueName)
                    ? Resources.NumberGame
                    : $"{Resources.NumberGame} - {leagueName}";
            }

            if (BetTypeHelper.IsHappy5(data.BetType))
            {
                return string.IsNullOrWhiteSpace(leagueName)
                    ? Resources.Happy5
                    : $"{Resources.Happy5} - {leagueName}";
            }

            return leagueName;
        }
    }
}