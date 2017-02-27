using System.Collections.Generic;
using Alpha.Reports.Models.WinlossByBetType;

namespace Alpha.Reports.Models.WinLossByProduct
{
    public class WinLossByProductModel
    {
        public WinLossByProductModel()
        {
        }

        public WinLossByProductModel(
            int productId,
            string productName,
            bool liveIndicator,
            int betCount,
            decimal grossComm,
            decimal turnover,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss,
            IEnumerable<WinLossByBetTypeModel> winLossByBetTypeData)
        {
            ProductId = productId;
            ProductName = productName;
            LiveIndicator = liveIndicator;

            GrossComm = grossComm;
            Turnover = turnover;
            BetCount = betCount;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;

            WinLossByBetTypeData = winLossByBetTypeData;
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public bool LiveIndicator { get; }
        
        public int BetCount { get; private set; }

        public decimal Turnover { get; private set; }

        public decimal GrossComm { get; private set; }

        public NetWinLoss MemberNetWinLoss { get; private set; }

        public NetWinLoss AgentNetWinLoss { get; private set; }

        public NetWinLoss MasterNetWinLoss { get; private set; }

        public NetWinLoss SuperNetWinLoss { get; private set; }

        public IEnumerable<WinLossByBetTypeModel> WinLossByBetTypeData { get; }

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

        public WinLossByProductModel Add(WinLossByProductModel winlossByBetType)
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