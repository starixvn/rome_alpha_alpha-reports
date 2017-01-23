namespace Alpha.Reports.Params
{
    using System;
    using Models;

    public class WinLossByBettypeParam
    {
        public WinLossByBettypeParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }
    }
}