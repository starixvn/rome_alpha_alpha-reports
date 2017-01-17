namespace Alpha.Reports.Site.ViewModels.MatchWinLossDetail
{
    using App_GlobalResources;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;

    public class BetTypeWinLossViewModel
    {
        public BetTypeWinLossViewModel(
            int id,
            string name,
            decimal turnover,
            decimal grossComm,
            bool isLive,
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
                winLoss.MemberNetWinLoss,
                winLoss.AgentNetWinLoss,
                winLoss.MasterNetWinLoss,
                winLoss.SuperNetWinLoss,
                winLoss.CompanyNetWinLoss)
        {
        }

        public int Id { get; }

        public string Name { get; }

        public decimal Turnover { get; }

        public decimal GrossComm { get; }

        public bool IsLive { get; }

        public NetWinLoss MemberNetWinLoss { get; }

        public NetWinLoss AgentNetWinLoss { get; }

        public NetWinLoss MasterNetWinLoss { get; }

        public NetWinLoss SuperNetWinLoss { get; }

        public NetWinLoss CompanyNetWinLoss { get; }

        public static BetTypeWinLossViewModel Add(BetTypeWinLossViewModel winlossA, BetTypeWinLossViewModel winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return new BetTypeWinLossViewModel(
                id: 0,
                name: Resources.SubTotal,
                turnover: winlossA.Turnover + winlossB.Turnover,
                grossComm: winlossA.GrossComm + winlossB.GrossComm,
                isLive: false,
                memberNetWinLoss: NetWinLoss.Add(winlossA.MemberNetWinLoss, winlossB.MemberNetWinLoss),
                agentNetWinLoss: NetWinLoss.Add(winlossA.AgentNetWinLoss, winlossB.AgentNetWinLoss),
                masterNetWinLoss: NetWinLoss.Add(winlossA.MasterNetWinLoss, winlossB.MasterNetWinLoss),
                superNetWinLoss: NetWinLoss.Add(winlossA.SuperNetWinLoss, winlossB.SuperNetWinLoss),
                companyNetWinLoss: NetWinLoss.Add(winlossA.CompanyNetWinLoss, winlossB.CompanyNetWinLoss));
        }
    }
}