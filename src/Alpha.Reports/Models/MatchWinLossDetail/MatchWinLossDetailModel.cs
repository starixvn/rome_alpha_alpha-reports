namespace Alpha.Reports.Models.MatchWinLossDetail
{
    using Models;

    public class MatchWinLossDetailModel
    {
        public MatchWinLossDetailModel()
        {

        }

        public MatchWinLossDetailModel(
            bool isParlay,
            int matchId,
            int homeId,
            int awayId,
            int betType,
            bool liveIndicator,
            int leagueId,
            int leagueGroupId,
            byte leagueType,
            int race,
            string matchCode,
            bool isNeutral,
            string showTime,
            decimal turnover,
            decimal grossComm,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss)
        {
            IsParlay = isParlay;
            MatchId = matchId;
            HomeId = homeId;
            AwayId = awayId;
            BetType = betType;
            LiveIndicator = liveIndicator;
            LeagueId = leagueId;
            LeagueGroupId = leagueGroupId;
            LeagueType = leagueType;
            Race = race;
            MatchCode = matchCode;
            IsNeutral = isNeutral;
            ShowTime = showTime;
            Turnover = turnover;
            GrossComm = grossComm;

            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;

            var companyComm =
                (memberNetWinLoss?.Comm ?? 0)
                + (agentNetWinLoss?.Comm ?? 0)
                + (masterNetWinLoss?.Comm ?? 0)
                + (superNetWinLoss?.Comm ?? 0);

            var winLoss =
                (memberNetWinLoss?.WinLoss ?? 0)
                + (agentNetWinLoss?.WinLoss ?? 0)
                + (masterNetWinLoss?.WinLoss ?? 0)
                + (superNetWinLoss?.WinLoss ?? 0);

            CompanyNetWinLoss = new NetWinLoss(0, companyComm, -winLoss - companyComm);
        }

        public bool IsParlay { get; }

        public int MatchId { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public int BetType { get; }

        public bool LiveIndicator { get; }

        public int LeagueId { get; }

        public int LeagueGroupId { get; }

        public byte LeagueType { get; }

        public int Race { get; }

        public string MatchCode { get; }

        public bool IsNeutral { get; }

        public string ShowTime { get; }

        public decimal Turnover { get; }

        public decimal GrossComm { get; }

        public NetWinLoss MemberNetWinLoss { get; }

        public NetWinLoss AgentNetWinLoss { get; }

        public NetWinLoss MasterNetWinLoss { get; }

        public NetWinLoss SuperNetWinLoss { get; }

        public NetWinLoss CompanyNetWinLoss { get; }
    }
}
