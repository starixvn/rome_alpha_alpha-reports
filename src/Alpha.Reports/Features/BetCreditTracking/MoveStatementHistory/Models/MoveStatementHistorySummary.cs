namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    public class MoveStatementHistorySummary
    {
        public MoveStatementHistorySummary()
        {
        }

        public MoveStatementHistorySummary(int totalMembers, decimal memberWinLoss, decimal memberComm, decimal memberTotal, decimal variance)
        {
            TotalMembers = totalMembers;
            MemberWinLoss = memberWinLoss;

            MemberComm = memberComm;
            MemberTotal = memberTotal;
            Variance = variance;
        }
        public int TotalMembers { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }

        public decimal Variance { get; set; }
    }
}
