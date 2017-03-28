namespace Alpha.Reports.Params
{
    using System;
    using Utils;

    public class MatchWinLossParam
    {
        public MatchWinLossParam(
            int custId,
            DateTime fromDate,
            DateTime toDate)
        {
            CustId = custId;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
