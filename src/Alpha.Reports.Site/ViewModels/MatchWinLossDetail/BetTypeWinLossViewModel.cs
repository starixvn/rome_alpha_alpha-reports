namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;

    public class BetTypeWinLossViewModel
    {
        public BetTypeWinLossViewModel()
        {
        }

        public BetTypeWinLossViewModel(
            int id,
            string name,
            decimal turnover,
            decimal grossComm,
            bool isLive,
            int race,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss,
            NetWinLoss companyNetWinLoss)
        {
            Id = id;
            Name = name;
            Turnover = turnover;
            GrossComm = grossComm;
            IsLive = isLive;
            Race = race;
            MemberNetWinLoss = memberNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
            CompanyNetWinLoss = companyNetWinLoss;
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

        public int Race { get; }

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
    }
}