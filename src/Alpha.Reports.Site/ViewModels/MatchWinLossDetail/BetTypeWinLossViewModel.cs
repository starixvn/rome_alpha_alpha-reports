namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using App_GlobalResources;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;

    public class BetTypeWinLossViewModel
    {
        public BetTypeWinLossViewModel()
        {
            Name = string.Empty;
        }

        public BetTypeWinLossViewModel(
            int id,
            string name,
            decimal turnover,
            decimal grossComm,
            bool isLive,
            string race,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss,
            NetWinLoss companyNetWinLoss)
        {
            Id = id;
            Turnover = turnover;
            GrossComm = grossComm;
            IsLive = isLive;
            Race = race;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
            CompanyNetWinLoss = companyNetWinLoss;

            Name = GetBetTypePrefix() + (name ?? string.Empty) + GetBetTypeSuffix();
        }

        public BetTypeWinLossViewModel(MatchWinLossDetailModel winLoss, string betTypeName) :
            this(
                winLoss.BetType,
                betTypeName,
                winLoss.Turnover,
                winLoss.GrossComm,
                winLoss.LiveIndicator,
                winLoss.Race,
                winLoss.MemberNetWinLoss,
                winLoss.AgentNetWinLoss,
                winLoss.MasterNetWinLoss,
                winLoss.SuperNetWinLoss,
                winLoss.CompanyNetWinLoss)
        {
        }

        public int Id { get; }

        public string Name { get; }

        public decimal Turnover { get; private set; }

        public decimal GrossComm { get; private set; }

        public bool IsLive { get; }

        public string Race { get; }

        public NetWinLoss MemberNetWinLoss { get; private set; }

        public NetWinLoss AgentNetWinLoss { get; private set; }

        public NetWinLoss MasterNetWinLoss { get; private set; }

        public NetWinLoss SuperNetWinLoss { get; private set; }

        public NetWinLoss CompanyNetWinLoss { get; private set; }

        public BetTypeWinLossViewModel Add(BetTypeWinLossViewModel winloss)
        {
            if (winloss == null)
            {
                return this;
            }

            Turnover += winloss.Turnover;
            GrossComm += winloss.GrossComm;

            MemberNetWinLoss = NetWinLoss.Add(MemberNetWinLoss, winloss.MemberNetWinLoss);
            AgentNetWinLoss = NetWinLoss.Add(AgentNetWinLoss, winloss.AgentNetWinLoss);
            MasterNetWinLoss = NetWinLoss.Add(MasterNetWinLoss, winloss.MasterNetWinLoss);
            SuperNetWinLoss = NetWinLoss.Add(SuperNetWinLoss, winloss.SuperNetWinLoss);
            CompanyNetWinLoss = NetWinLoss.Add(CompanyNetWinLoss, winloss.CompanyNetWinLoss);

            return this;
        }

        private string GetBetTypePrefix()
        {
            switch (Id)
            {
                case BetTypeIdConstants.NumberGameOverUnder:
                    return $"{Resources.Next} ";

                case BetTypeIdConstants.NumberGameOddEven:
                    return IsLive ? $"{Resources.Next} " : $"{Resources.FT} ";

                default:
                    return string.Empty;
            }
        }

        private string GetBetTypeSuffix()
        {
            if (Id == BetTypeIdConstants.MixParlayBetRadarVirtualSports || Id == BetTypeIdConstants.MixParlayEdgeGamingVirtualSports || Id == BetTypeIdConstants.MixParlayVirtuallySports)
            {
                return $" ({Resources.VirtualSports})";
            }

            return string.Empty;
        }
    }
}