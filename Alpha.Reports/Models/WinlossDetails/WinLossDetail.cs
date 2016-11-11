namespace Alpha.Reports.Models.WinlossDetails
{
    public class WinLossDetail
    {
        public WinLossDetail()
        {
        }

        public WinLossDetail(WinLossDetailDto dto)
        {
            CustId = dto.CustId;
            CustName = dto.CustName;
            GrossComm = dto.GrossComm;
            NetTurnover = dto.NetTurnover;
            Turnover = dto.Turnover;
            NetTurnover = dto.NetTurnover;
            NumberOfTickets = dto.NumberOfTickets;

            Member = new CustWinLoss(dto.MemberWinLoss, dto.MemberComm);

            Agent = new CustWinLoss(dto.AgWinLoss, dto.AgComm);

            Master = dto.MAgWinLoss.HasValue
                ? new CustWinLoss(dto.MAgWinLoss.Value, dto.MAgComm)
                : null;

            Super = dto.SAgWinLoss.HasValue
                ? new CustWinLoss(dto.SAgWinLoss.Value, dto.SAgComm)
                : null;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public int NumberOfTickets { get; set; }

        public CustWinLoss Member { get; set; }

        public CustWinLoss Agent { get; set; }

        public CustWinLoss Master { get; set; }

        public CustWinLoss Super { get; set; }

        public decimal Company =>
            -(Member.Total
            + Agent.Total
            + (Master?.Total ?? 0)
            + (Super?.Total ?? 0));
    }
}