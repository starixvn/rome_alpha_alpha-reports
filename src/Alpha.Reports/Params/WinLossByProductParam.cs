namespace Alpha.Reports.Params
{
    using System;
    using Models;
    using Utils;

    public class WinLossByProductParam
    {
        public WinLossByProductParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;
            if (fromDate == DateTime.MinValue)
            {
                FromDate = SystemTime.Today;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }
    }
}