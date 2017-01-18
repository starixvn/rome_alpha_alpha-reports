namespace Alpha.Reports.Params
{
    using System;
    using Services;

    public class MatchWinLossParam
    {
        public MatchWinLossParam(
            int custId,
            DateTime fromDate,
            DateTime toDate)
        {
            CustId = custId;
            FromDate = fromDate == DateTime.MinValue ? DateTimeManager.Now : fromDate;
            ToDate = toDate == DateTime.MinValue ? DateTimeManager.Now : toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
