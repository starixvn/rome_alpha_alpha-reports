namespace Alpha.Reports.Models.WinlossDetail
{
    public class MemberWinlossDetail : BaseWinlossDetail
    {
        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal AgentWinLoss { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentTotal { get; set; }
    }
}
