namespace Alpha.Reports.Models.WinlossDetail
{
    public class MemberWinloss
    {
        public MemberWinloss(WinlossDetailInfo info, Amount memberAmount, Amount agentAmount)
        {
            Info = info;
            MemberAmount = memberAmount;
            AgentAmount = agentAmount;
        }

        public WinlossDetailInfo Info { get; }

        public Amount MemberAmount { get; }

        public Amount AgentAmount { get; }        

        public decimal Company
        {
            get
            {
                return -(MemberAmount.Value + AgentAmount.Value);
            }
        }
    }
}
