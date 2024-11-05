namespace Alpha.Reports.Models.WinLossDetail
{
    public class WinLossDetailEntity
    {
        public int? CustId { get; set; }

        public string CustName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public int NumberOfTickets { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal AgWinLoss { get; set; }

        public decimal AgComm { get; set; }

        public decimal? MAgWinLoss { get; set; }

        public decimal? MAgComm { get; set; }

        public decimal? SAgWinLoss { get; set; }

        public decimal? SAgComm { get; set; }

        public bool? IsExistedWatchList { get; set; }

        public decimal? RewardAmount { get; set; }

        public string RewardCurrency { get; set; }

        public bool IsDirectMember { get; set; }
    }
}
