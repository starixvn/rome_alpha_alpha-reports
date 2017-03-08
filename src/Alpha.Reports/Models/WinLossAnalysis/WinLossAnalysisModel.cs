namespace Alpha.Reports.Models.WinLossAnalysis
{
    public class WinLossAnalysisModel
    {
        public WinLossAnalysisModel()
        {
        }

        public WinLossAnalysisModel(
            string rollupKey,
            decimal betCount,
            decimal turnover,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss)
        {
            RollupKey = rollupKey;
            BetCount = betCount;
            Turnover = turnover;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
        }

        public string RollupKey { get; }

        public decimal Turnover { get; private set; }

        public decimal BetCount { get; private set; }

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

        public WinLossAnalysisModel Add(WinLossAnalysisModel winloss)
        {
            if (winloss == null)
            {
                return this;
            }

            BetCount += winloss.BetCount;
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