namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params
{
    using System;

    public class MoveStatementHistoryParam
    {
        public MoveStatementHistoryParam(string memberUsername, DateTime fromDate, DateTime toDate, int viewerId, DateTime limitedDateToShowInfo)
        {
            MemberUsername = memberUsername;
            FromDate = fromDate;
            ToDate = toDate;
            ViewerId = viewerId;
            LimitedDateToShowInfo = limitedDateToShowInfo;
        }

        public string MemberUsername { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ViewerId { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}
