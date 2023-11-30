namespace Alpha.Reports.Models.MatchWinLossDetail
{
    public class MatchWinLossDetailEntity
    {
        public int BetGroup { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int BetType { get; set; }

        public bool LiveIndicator { get; set; }

        public int LeagueId { get; set; }

        public int LeagueGroupId { get; set; }

        public byte LeagueType { get; set; }

        public string Race { get; set; }

        public string MatchCode { get; set; }

        public bool IsNeutral { get; set; }

        public string ShowTime { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal AgWinLoss { get; set; }

        public decimal AgComm { get; set; }

        public decimal MAgWinLoss { get; set; }

        public decimal MAgComm { get; set; }

        public decimal SAgWinLoss { get; set; }

        public decimal SAgComm { get; set; }
    }
}
