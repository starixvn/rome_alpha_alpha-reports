namespace Alpha.Reports.Models.WinlossDetail
{
    public class MasterWinLossDetail : AgentWinLossDetail
    {
        public decimal SuperWinLoss { get; set; }

        public decimal SuperComm { get; set; }

        public decimal SuperTotal => SuperWinLoss + SuperComm;

        public override decimal Company => -(MemberTotal + AgentTotal + MasterTotal + SuperTotal);
    }
}
