using Alpha.Reports.Models.WinLossByProduct;

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

        public string BetTypeName { get; }

        public int ProductId { get; }

        public bool LiveIndicator { get; }
        
        public int BetCount { get; private set; }

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

        public WinLossByBetTypeModel Add(WinLossByBetTypeModel winlossByBetType)
        {
            if (winlossByBetType == null)
            {
                return this;
            }

            BetCount += winlossByBetType.BetCount;
            GrossComm += winlossByBetType.GrossComm;
            Turnover += winlossByBetType.Turnover;
            Company += winlossByBetType.Company;
            MemberNetWinLoss = NetWinLoss.Add(MemberNetWinLoss, winlossByBetType.MemberNetWinLoss);
            AgentNetWinLoss = NetWinLoss.Add(AgentNetWinLoss, winlossByBetType.AgentNetWinLoss);
            MasterNetWinLoss = NetWinLoss.Add(MasterNetWinLoss, winlossByBetType.MasterNetWinLoss);
            SuperNetWinLoss = NetWinLoss.Add(SuperNetWinLoss, winlossByBetType.SuperNetWinLoss);

            return this;
        }
    }
}