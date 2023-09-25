namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models
{
    using System;

    public class MoveStatementHistorySummary
    {
        public int TotalMembers { get;  }

        public decimal MemberWinLoss { get; }

        public decimal MemberComm { get; }

        public decimal MemberTotal { get; }
        
        public decimal Variance { get; }
    }
}
