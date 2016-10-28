namespace Alpha.Reports.Models.WinlossDetail
{
    public class MemberWinlossDetail : WinlossDetailInfo
    {
        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal
        {
            get { return MemberWinLoss + MemberComm; }
        }

        public decimal AgentWinLoss { get; }

        public decimal AgentComm { get; }

        public decimal AgentTotal
        {
            get { return AgentWinLoss + AgentComm; }
        }

        public decimal Company
        {
            get
            {
                return -(MemberTotal + AgentTotal);
            }
        }
    }
}
