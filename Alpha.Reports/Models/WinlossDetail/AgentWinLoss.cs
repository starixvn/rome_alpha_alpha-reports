namespace Alpha.Reports.Models.WinlossDetail
{
    public class AgentWinLoss
    {
        public AgentWinLoss(WinlossDetailInfo info, Amount memberAmount, Amount agentAmount, Amount masterAmount)
        {
            Info = info;
            MemberAmount = memberAmount;
            AgentAmount = agentAmount;
            MasterAmount = masterAmount;        
        }

        public WinlossDetailInfo Info { get; }

        public Amount MemberAmount { get; }

        public Amount AgentAmount { get; }

        public Amount MasterAmount { get; }

        public decimal Company
        {
            get
            {
                return -(MemberAmount.Value + AgentAmount.Value + MasterAmount.Value);
            }
        }
    }
}
