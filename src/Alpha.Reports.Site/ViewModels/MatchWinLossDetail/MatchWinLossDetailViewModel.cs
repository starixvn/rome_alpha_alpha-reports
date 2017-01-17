namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using System.Collections.Generic;

    public class MatchWinLossDetailViewModel
    {
        public MatchWinLossDetailViewModel(
            int leagueId,
            string leagueName,
            int matchId,
            string matchName,
            int race,
            IEnumerable<BetTypeWinLossViewModel> betTypeWinLoss,
            BetTypeWinLossViewModel total)
        {
            MatchId = matchId;
            MatchName = matchName;
            LeagueId = leagueId;
            LeagueName = leagueName;
            Race = race;
            BetTypeWinLoss = betTypeWinLoss;
            Total = total;
        }

        public int LeagueId { get; }

        public string LeagueName { get; }

        public int MatchId { get; }

        public string MatchName { get; }

        public int Race { get; }

        public IEnumerable<BetTypeWinLossViewModel> BetTypeWinLoss { get; }

        public BetTypeWinLossViewModel Total { get; }
    }
}