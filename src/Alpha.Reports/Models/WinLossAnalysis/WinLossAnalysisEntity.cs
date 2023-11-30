namespace Alpha.Reports.Models.WinLossAnalysis
{
    public class WinLossAnalysisEntity
    {
        public string RollupKey { get; set; }

        public decimal Turnover { get; set; }

        public int BetCount { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal AgWinLoss { get; set; }

        public decimal AgComm { get; set; }

        public decimal? MAgWinLoss { get; set; }

        public decimal? MAgComm { get; set; }

        public decimal? SAgWinLoss { get; set; }

        public decimal? SAgComm { get; set; }
    }
}
