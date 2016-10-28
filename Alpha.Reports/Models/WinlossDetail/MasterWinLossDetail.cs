namespace Alpha.Reports.Models.WinlossDetail
{
    public class MasterWinLossDetail : WinlossDetailInfo
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

        public decimal SuperWinLoss { get; }

        public decimal SuperComm { get; }

        public decimal SuperTotal
        {
            get { return SuperWinLoss + SuperComm; }
        }

        public decimal Company
        {
            get
            {
                return -(MemberTotal + AgentTotal + SuperTotal + SuperTotal);
            }
        }
    }
}
