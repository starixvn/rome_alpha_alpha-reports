namespace Alpha.Reports.Models.Winloss
{
    using WinlossDetails;

    public class WinLoss
    {
        public WinLoss()
        {
        }

        public WinLoss(
            int custId,
            string custName,
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
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
        }

        public int CustId { get; }

        public string CustName { get; }

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

        public static WinLoss Add(WinLoss winlossA, WinLoss winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return new WinLoss(
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