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

        public static WinlossByBettypeModel Add(WinlossByBettypeModel winlossByBettypeA, WinlossByBettypeModel winlossByBettypeB)
        {
            if (winlossByBettypeA == null || winlossByBettypeB == null)
            {
                return winlossByBettypeA ?? winlossByBettypeB;
            }

            return new WinlossByBettypeModel(
                custId: 0,
                custName: "Total",
                numberOfTickets: winlossByBettypeA.NumberOfTickets + winlossByBettypeB.NumberOfTickets,
                grossComm: winlossByBettypeA.GrossComm + winlossByBettypeB.GrossComm,
                turnover: winlossByBettypeA.Turnover + winlossByBettypeB.Turnover,
                memberNetWinLoss: NetWinLoss.Add(winlossByBettypeA.MemberNetWinLoss, winlossByBettypeB.MemberNetWinLoss),
                agentNetWinLoss: NetWinLoss.Add(winlossByBettypeA.AgentNetWinLoss, winlossByBettypeB.AgentNetWinLoss),
                masterNetWinLoss: NetWinLoss.Add(winlossByBettypeA.MasterNetWinLoss, winlossByBettypeB.MasterNetWinLoss),
                superNetWinLoss: NetWinLoss.Add(winlossByBettypeA.SuperNetWinLoss, winlossByBettypeB.SuperNetWinLoss));
        }
    }
}