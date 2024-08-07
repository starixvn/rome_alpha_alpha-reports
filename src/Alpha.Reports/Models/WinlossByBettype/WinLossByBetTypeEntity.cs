namespace Alpha.Reports.Models.WinlossByBetType
{
    public class WinLossByBetTypeEntity
    {
        public int BetType { get; set; }

        public int? ProductId { get; set; }

        public int? SportType { get; set; }

        public bool LiveIndicator { get; set; }

        public bool IsMMR { get; set; }

        public decimal GrossComm { get; set; }

        public decimal Turnover { get; set; }

        public int BetCount { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal DownlineTotal { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Comm { get; set; }
    }
}
