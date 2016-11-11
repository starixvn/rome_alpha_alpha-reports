namespace Alpha.Reports.Models.WinlossDetails
{
    public class WinLossDetail
    {
        public WinLossDetail()
        {
        }

        public WinLossDetail(
            int custId,
            string custName,
            decimal grossComm,
            decimal netTurnover,
            decimal turnover,
            int numberOfTickets,
            CustWinLoss memberWinLoss,
            CustWinLoss agentWinLoss,
            CustWinLoss masterWinLoss,
            CustWinLoss superWinLoss)
        {
            CustId = custId;
            CustName = custName;
            GrossComm = grossComm;
            NetTurnover = netTurnover;
            Turnover = turnover;
            NumberOfTickets = numberOfTickets;
            Member = memberWinLoss;
            Agent = agentWinLoss;
            Master = masterWinLoss;
            Super = superWinLoss;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal Turnover { get; }

        public decimal NetTurnover { get; }

        public decimal GrossComm { get; }

        public int NumberOfTickets { get; }

        public CustWinLoss Member { get; }

        public CustWinLoss Agent { get; }

        public CustWinLoss Master { get; }

        public CustWinLoss Super { get; }

        public decimal Company =>
            -(Member.Total
            + Agent.Total
            + (Master?.Total ?? 0)
            + (Super?.Total ?? 0));
    }
}