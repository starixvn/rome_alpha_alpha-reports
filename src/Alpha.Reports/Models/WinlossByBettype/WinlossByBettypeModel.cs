namespace Alpha.Reports.Models.WinlossByBettype
{
    using Winloss;
    using WinLossDetail;

    public class WinlossByBettypeModel
    {
        public WinlossByBettypeModel()
        {
        }

        public WinlossByBettypeModel(
            int custId,
            string custName,
            int numberOfTickets,
            decimal grossComm,
            decimal turnover,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss)
        {
            CustId = custId;
            CustName = custName;
            GrossComm = grossComm;
            Turnover = turnover;
            NumberOfTickets = numberOfTickets;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
        }

        public int CustId { get; }

        public string CustName { get; }

        public int NumberOfTickets { get; }

        public decimal Turnover { get; }

        public decimal GrossComm { get; }

        public NetWinLoss MemberNetWinLoss { get; }

        public NetWinLoss AgentNetWinLoss { get; }

        public NetWinLoss MasterNetWinLoss { get; }

        public NetWinLoss SuperNetWinLoss { get; }

        public decimal Company =>
            -((MemberNetWinLoss?.Total ?? 0)
            + (AgentNetWinLoss?.Total ?? 0)
            + (MasterNetWinLoss?.Total ?? 0)
            + (SuperNetWinLoss?.Total ?? 0));

        public static WinLossModel Add(WinLossModel winlossA, WinLossModel winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return new WinLossModel(
                custId: 0,
                custName: "Total",
                grossComm: winlossA.GrossComm + winlossB.GrossComm,
                turnover: winlossA.Turnover + winlossB.Turnover,
                memberNetWinLoss: NetWinLoss.Add(winlossA.MemberNetWinLoss, winlossB.MemberNetWinLoss),
                agentNetWinLoss: NetWinLoss.Add(winlossA.AgentNetWinLoss, winlossB.AgentNetWinLoss),
                masterNetWinLoss: NetWinLoss.Add(winlossA.MasterNetWinLoss, winlossB.MasterNetWinLoss),
                superNetWinLoss: NetWinLoss.Add(winlossA.SuperNetWinLoss, winlossB.SuperNetWinLoss));
        }
    }
}