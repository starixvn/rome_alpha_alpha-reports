namespace Alpha.Reports.Params
{
    using System;

    public class RiskAnalysisIPMatchingParam
    {
        public RiskAnalysisIPMatchingParam(int custId, DateTime winlostDate, DateTime moveBetsDay, int memberCount)
        {
            CustId = custId;
            WinlostDate = winlostDate.Date;
            MoveBetsDay = moveBetsDay.Date;
            MemberCount = memberCount;
        }

        public int CustId { get; }

        public DateTime WinlostDate { get; }

        public DateTime MoveBetsDay { get; }

        public int MemberCount { get; }
    }
}
