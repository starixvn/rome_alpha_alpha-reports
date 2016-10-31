namespace Alpha.Reports.Models.WinlossDetail
{
    public class MasterWinLossDetail : AgentWinLossDetail
    {
        public decimal SuperWinLoss { get; }

        public decimal SuperComm { get; }

        public decimal SuperTotal => SuperWinLoss + SuperComm;

        public override decimal Company => -(MemberTotal + AgentTotal + MasterTotal + SuperTotal);
    }
}
