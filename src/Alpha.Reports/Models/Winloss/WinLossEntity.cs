namespace Alpha.Reports.Models.Winloss
{
    public class WinLossEntity
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal DownlineTotal { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Comm { get; set; }
    }
}
