namespace Alpha.Reports.Models.WinlossDetails
{
    public class WinLossDetailDto
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal AgWinLoss { get; set; }

        public decimal AgComm { get; set; }

        public decimal? MAgWinLoss { get; set; }

        public decimal MAgComm { get; set; }

        public decimal? SAgWinLoss { get; set; }

        public decimal SAgComm { get; set; }

        public int NumberOfTickets { get; set; }

        public long RowNum { get; set; }
    }
}
