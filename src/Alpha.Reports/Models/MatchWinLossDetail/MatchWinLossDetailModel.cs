namespace Alpha.Reports.Models.MatchWinLossDetail
{
    using Models;

    public class MatchWinLossDetailModel
    {
        public MatchWinLossDetailModel(
            bool isParlay,
            int matchId,
            int homeId,
            int awayId,
            int betType,
            bool liveIndicator,
            int leagueId,
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
                (memberNetWinLoss != null ? memberNetWinLoss.Comm.Value : 0) +
                (agentNetWinLoss != null ? agentNetWinLoss.Comm.Value : 0) +
                (masterNetWinLoss != null ? masterNetWinLoss.Comm.Value : 0) +
                (superNetWinLoss != null ? superNetWinLoss.Comm.Value : 0);
            var winLoss =
                (memberNetWinLoss != null ? memberNetWinLoss.WinLoss.Value : 0) +
                (agentNetWinLoss != null ? agentNetWinLoss.WinLoss.Value : 0) +
                (masterNetWinLoss != null ? masterNetWinLoss.WinLoss.Value : 0) +
                (superNetWinLoss != null ? superNetWinLoss.WinLoss.Value : 0);

            CompanyNetWinLoss = new NetWinLoss(0, companyComm, -winLoss - companyComm);
        }

        public bool IsParlay { get; }

        public int MatchId { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public int BetType { get; }

        public bool LiveIndicator { get; }

        public int LeagueId { get; }

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
