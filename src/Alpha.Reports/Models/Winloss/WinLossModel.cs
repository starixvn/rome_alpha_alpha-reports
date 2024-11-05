namespace Alpha.Reports.Models.Winloss
{
    public class WinLossModel
    {
        public WinLossModel()
        {
        }

        public WinLossModel(
            int custId,
            string custName,
            decimal grossComm,
            decimal turnover,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss,
            bool isDirectMember)
        {
            CustId = custId;
            CustName = custName;
            GrossComm = grossComm;
            Turnover = turnover;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
            IsDirectMember = isDirectMember;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal Turnover { get; private set; }

        public decimal GrossComm { get; private set; }

        public NetWinLoss MemberNetWinLoss { get; private set; }

        public NetWinLoss AgentNetWinLoss { get; private set; }

        public NetWinLoss MasterNetWinLoss { get; private set; }

        public NetWinLoss SuperNetWinLoss { get; private set; }

        public decimal Company
        {
            get
            {
                return -((MemberNetWinLoss?.Total ?? 0)
                        + (AgentNetWinLoss?.Total ?? 0)
                        + (MasterNetWinLoss?.Total ?? 0)
                        + (SuperNetWinLoss?.Total ?? 0));
            }

            private set
            {
            }
        }

        public bool IsDirectMember { get; private set; }

        public WinLossModel Add(WinLossModel winloss)
        {
            if (winloss == null)
            {
                return this;
            }

            GrossComm += winloss.GrossComm;
            Turnover += winloss.Turnover;
            Company += winloss.Company;
            MemberNetWinLoss = NetWinLoss.Add(MemberNetWinLoss, winloss.MemberNetWinLoss);
            AgentNetWinLoss = NetWinLoss.Add(AgentNetWinLoss, winloss.AgentNetWinLoss);
            MasterNetWinLoss = NetWinLoss.Add(MasterNetWinLoss, winloss.MasterNetWinLoss);
            SuperNetWinLoss = NetWinLoss.Add(SuperNetWinLoss, winloss.SuperNetWinLoss);

            return this;
        }
    }
}