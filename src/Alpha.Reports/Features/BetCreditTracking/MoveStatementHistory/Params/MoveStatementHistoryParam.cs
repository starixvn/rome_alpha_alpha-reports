namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params
{
    using System;

    public class MoveStatementHistoryParam
    {
        public MoveStatementHistoryParam(string memberUsername, DateTime fromDate, DateTime toDate, DateTime limitedDateToShowInfo)
        {
            MemberUsername = memberUsername;
            FromDate = fromDate;
            ToDate = toDate;
            LimitedDateToShowInfo = limitedDateToShowInfo;
        }

        public string MemberUsername { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}
