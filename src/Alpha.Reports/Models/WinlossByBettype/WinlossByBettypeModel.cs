namespace Alpha.Reports.Models.WinlossByBetType
{
    public class WinLossByBetTypeModel
    {
        public WinLossByBetTypeModel()
        {
        }

        public WinLossByBetTypeModel(
            int betTypeId,
            string betTypeName,
            int productId,
            bool liveIndicator,
            int betCount,
            decimal grossComm,
            decimal turnover,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss)
        {
            BetTypeId = betTypeId;
            BetTypeName = betTypeName;
            ProductId = productId;
            LiveIndicator = liveIndicator;

            GrossComm = grossComm;
            Turnover = turnover;
            BetCount = betCount;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
        }

        public int BetTypeId { get; }

        public string BetTypeName { get; set; }

        public int ProductId { get; }

        public bool IsAllowViewGameType { get; set; }

        public bool LiveIndicator { get; }
        
        public int BetCount { get; }

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

        public static WinLossByBetTypeModel Add(WinLossByBetTypeModel winlossByBettypeA, WinLossByBetTypeModel winlossByBettypeB)
        {
            if (winlossByBettypeA == null || winlossByBettypeB == null)
            {
                return winlossByBettypeA ?? winlossByBettypeB;
            }

            return new WinLossByBetTypeModel(
                betTypeId: 0,
                betTypeName: string.Empty,
                productId: 0,
                liveIndicator: false,
                betCount: winlossByBettypeA.BetCount + winlossByBettypeB.BetCount,
                grossComm: winlossByBettypeA.GrossComm + winlossByBettypeB.GrossComm,
                turnover: winlossByBettypeA.Turnover + winlossByBettypeB.Turnover,
                memberNetWinLoss: NetWinLoss.Add(winlossByBettypeA.MemberNetWinLoss, winlossByBettypeB.MemberNetWinLoss),
                agentNetWinLoss: NetWinLoss.Add(winlossByBettypeA.AgentNetWinLoss, winlossByBettypeB.AgentNetWinLoss),
                masterNetWinLoss: NetWinLoss.Add(winlossByBettypeA.MasterNetWinLoss, winlossByBettypeB.MasterNetWinLoss),
                superNetWinLoss: NetWinLoss.Add(winlossByBettypeA.SuperNetWinLoss, winlossByBettypeB.SuperNetWinLoss));
        }
    }
}