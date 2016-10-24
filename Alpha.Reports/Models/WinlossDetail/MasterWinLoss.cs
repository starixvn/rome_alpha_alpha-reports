namespace Alpha.Reports.Models.WinlossDetail
{
    public class MasterWinLoss : WinlossDetailInfo
    {
        public MasterWinLoss(
            WinlossDetailInfo info, 
            Amount memberAmount, 
            Amount agentAmount, 
            Amount masterAmount,
            Amount superAmount)
        {
            MemberAmount = memberAmount;
            AgentAmount = agentAmount;
            MasterAmount = masterAmount;
            SuperAmount = superAmount;
        }

        public Amount MemberAmount { get; }

        public Amount AgentAmount { get; }

        public Amount MasterAmount { get; }

        public Amount SuperAmount { get; }

        public decimal Company
        {
            get
            {
                return -(MemberAmount.Value + AgentAmount.Value + MasterAmount.Value + SuperAmount.Value);
            }
        }
    }
}
