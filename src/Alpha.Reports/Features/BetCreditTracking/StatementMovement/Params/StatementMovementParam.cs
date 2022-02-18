namespace Alpha.Reports.Features.BetCreditTracking.StatementMovement.Params
{
    using System;

    public class StatementMovementParam
    {
        public StatementMovementParam(string memberUsername, DateTime fromDate, DateTime toDate)
        {
            MemberUsername = memberUsername;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string MemberUsername { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
