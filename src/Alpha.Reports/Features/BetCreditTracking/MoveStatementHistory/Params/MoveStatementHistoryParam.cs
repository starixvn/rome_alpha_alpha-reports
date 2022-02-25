namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params
{
    using System;

    public class MoveStatementHistoryParam
    {
        public MoveStatementHistoryParam(string memberUsername, DateTime fromDate, DateTime toDate, DateTime limitedDate)
        {
            MemberUsername = memberUsername;
            FromDate = fromDate;
            ToDate = toDate;
            LimitedDate = limitedDate;
        }

        public string MemberUsername { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDate { get; }
    }
}
