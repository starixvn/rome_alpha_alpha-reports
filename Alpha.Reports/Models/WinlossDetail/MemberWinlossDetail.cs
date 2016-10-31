namespace Alpha.Reports.Models.WinlossDetail
{
    public class MemberWinlossDetail : WinLossDetailInfo
    {
        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal => MemberWinLoss + MemberComm;

        public decimal AgentWinLoss { get; }

        public decimal AgentComm { get; }

        public decimal AgentTotal => AgentWinLoss + AgentComm;

        public override decimal Company => -MemberTotal - AgentTotal;
    }
}
