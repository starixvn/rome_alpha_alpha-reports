namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params
{
    using System;

    public class BetCreditHistoryParam
    {
        public BetCreditHistoryParam(string member, long? refNo, DateTime fromDate, DateTime toDate, int viewerId)
        {
            Member = member;
            RefNo = refNo;
            FromDate = fromDate;
            ToDate = toDate;
            ViewerId = viewerId;
        }

        public string Member { get; }

        public long? RefNo { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ViewerId { get; }
    }
}
