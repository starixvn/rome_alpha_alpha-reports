namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params
{
    using System;

    public class BetCreditHistoryParam
    {
        public BetCreditHistoryParam(string member, string refNo, DateTime fromDate, DateTime toDate)
        {
            Member = member;
            RefNo = refNo;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string Member { get; }

        public string RefNo { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
