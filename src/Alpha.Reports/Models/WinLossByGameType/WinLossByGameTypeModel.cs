namespace Alpha.Reports.Models.WinlossByGameType
{
    public class WinLossByGameTypeModel
    {
        public WinLossByGameTypeModel()
        {
        }

        public WinLossByGameTypeModel(
            int gameTypeId,
            string gameTypeName,
            decimal turnover,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss)
        {
            GameTypeId = gameTypeId;
            GameTypeName = gameTypeName;
            Turnover = turnover;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
        }

        public int GameTypeId { get; }

        public string GameTypeName { get; }

        public decimal Turnover { get; private set; }

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

        public WinLossByGameTypeModel Add(WinLossByGameTypeModel winlossByGameType)
        {
            if (winlossByGameType == null)
            {
                return this;
            }

            Turnover += winlossByGameType.Turnover;
            Company += winlossByGameType.Company;
            MemberNetWinLoss = NetWinLoss.Add(MemberNetWinLoss, winlossByGameType.MemberNetWinLoss);
            AgentNetWinLoss = NetWinLoss.Add(AgentNetWinLoss, winlossByGameType.AgentNetWinLoss);
            MasterNetWinLoss = NetWinLoss.Add(MasterNetWinLoss, winlossByGameType.MasterNetWinLoss);
            SuperNetWinLoss = NetWinLoss.Add(SuperNetWinLoss, winlossByGameType.SuperNetWinLoss);

            return this;
        }
    }
}