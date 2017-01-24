namespace Alpha.Reports.Params
{
    using System;
    using Models;
    using Services;

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
            if (fromDate == DateTime.MinValue)
            {
                FromDate = DateTimeManager.Now;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? DateTimeManager.Now : toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }
    }
}