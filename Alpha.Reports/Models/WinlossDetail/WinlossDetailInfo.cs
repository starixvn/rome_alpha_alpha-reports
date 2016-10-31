namespace Alpha.Reports.Models.WinlossDetail
{
    public abstract class WinLossDetailInfo
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public int NumberOfTickets { get; set; }

        public abstract decimal Company { get; }
    }
}
