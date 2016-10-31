namespace Alpha.Reports.Models.WinlossDetail
{
    public class AgentWinLossDetail : MemberWinlossDetail
    {
        public decimal MasterWinLoss { get; }

        public decimal MasterComm { get; }

        public decimal MasterTotal => MasterWinLoss + MasterComm; 

        public override decimal Company => -MemberTotal - AgentTotal - MasterTotal;
    }
}
