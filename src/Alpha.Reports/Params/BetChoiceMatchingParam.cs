namespace Alpha.Reports.Params
{
    using System;

    public class BetChoiceMatchingParam
    {
        public BetChoiceMatchingParam(int custId, DateTime winlostDate, DateTime moveBetsDay, int betCount)
        {
            CustId = custId;
            WinlostDate = winlostDate.Date;
            MoveBetsDay = moveBetsDay.Date;
            BetCount = betCount;
        }

        public int CustId { get; }

        public DateTime WinlostDate { get; }

        public DateTime MoveBetsDay { get; }

        public int BetCount { get; }
    }
}
