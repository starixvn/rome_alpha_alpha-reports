namespace Alpha.Reports.Models.WinlossDetail
{
    public class AgentWinLossDetail : MemberWinLossDetail
    {
        public decimal MasterWinLoss { get; set; }

        public decimal MasterComm { get; set; }

        public decimal MasterTotal => MasterWinLoss + MasterComm;

        public override decimal Company => -(MemberTotal + AgentTotal + MasterTotal);
    }
}
