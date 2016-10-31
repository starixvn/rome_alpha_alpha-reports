namespace Alpha.Reports.Models.WinlossDetail
{
    public class MemberWinLossDetail
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public int NumberOfTickets { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal => MemberWinLoss + MemberComm;

        public decimal AgentWinLoss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal => AgentWinLoss + AgentComm;

        public virtual decimal Company => -(MemberTotal + AgentTotal);
    }
}
