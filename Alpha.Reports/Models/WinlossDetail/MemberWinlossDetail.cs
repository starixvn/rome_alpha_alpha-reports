namespace Alpha.Reports.Models.WinlossDetail
{
    public class MemberWinlossDetail
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnover { get; set; }

        public decimal GrossComm { get; set; }

        public int NumberOfTickets { get; set; }        

        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal => MemberWinLoss + MemberComm;

        public decimal AgentWinLoss { get; }

        public decimal AgentComm { get; }

        public decimal AgentTotal => AgentWinLoss + AgentComm;

        public virtual decimal Company => -(MemberTotal + AgentTotal);
    }
}
