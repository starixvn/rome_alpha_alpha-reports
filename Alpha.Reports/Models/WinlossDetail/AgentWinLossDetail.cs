namespace Alpha.Reports.Models.WinlossDetail
{
    public class AgentWinLossDetail : WinlossDetailInfo
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

        public decimal MasterWinLoss { get; }

        public decimal MasterComm { get; }

        public decimal MasterTotal
        {
            get { return MasterWinLoss + MasterComm; }
        }

        public decimal Company
        {
            get
            {
                return -(MemberTotal + AgentTotal + MasterTotal);
            }
        }
    }
}
