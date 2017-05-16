namespace Alpha.Reports.Params
{
    using System;
    using Models;

    public class CommissionByBettypeParam
    {
        public CommissionByBettypeParam(UserLevel userLevel, UserLevel custLevel, int custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay)
        {
            UserLevel = userLevel;
            CustLevel = custLevel;
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
        }

        public UserLevel UserLevel { get; }

        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }
    }
}
