namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System;

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
        public int TotalMembers { get; }

        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal { get; }

        public decimal Variance { get; }
    }
}
