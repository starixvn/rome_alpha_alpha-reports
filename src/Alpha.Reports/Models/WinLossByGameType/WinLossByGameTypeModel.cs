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

        public decimal Turnover { get; }

        public NetWinLoss MemberNetWinLoss { get; }

        public NetWinLoss AgentNetWinLoss { get; }

        public NetWinLoss MasterNetWinLoss { get; }

        public NetWinLoss SuperNetWinLoss { get; }

        public decimal Company =>
            -((MemberNetWinLoss?.Total ?? 0)
            + (AgentNetWinLoss?.Total ?? 0)
            + (MasterNetWinLoss?.Total ?? 0)
            + (SuperNetWinLoss?.Total ?? 0));

        public static WinLossByGameTypeModel Add(WinLossByGameTypeModel winlossByGameTypeA, WinLossByGameTypeModel winlossByGameTypeB)
        {
            if (winlossByGameTypeA == null || winlossByGameTypeB == null)
            {
                return winlossByGameTypeA ?? winlossByGameTypeB;
            }

            return new WinLossByGameTypeModel(
                gameTypeId: 0,
                gameTypeName: string.Empty,
                turnover: winlossByGameTypeA.Turnover + winlossByGameTypeB.Turnover,
                memberNetWinLoss: NetWinLoss.Add(winlossByGameTypeA.MemberNetWinLoss, winlossByGameTypeB.MemberNetWinLoss),
                agentNetWinLoss: NetWinLoss.Add(winlossByGameTypeA.AgentNetWinLoss, winlossByGameTypeB.AgentNetWinLoss),
                masterNetWinLoss: NetWinLoss.Add(winlossByGameTypeA.MasterNetWinLoss, winlossByGameTypeB.MasterNetWinLoss),
                superNetWinLoss: NetWinLoss.Add(winlossByGameTypeA.SuperNetWinLoss, winlossByGameTypeB.SuperNetWinLoss));
        }
    }
}