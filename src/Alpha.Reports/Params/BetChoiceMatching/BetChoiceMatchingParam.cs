namespace Alpha.Reports.Params.BetChoiceMatching
{
    using System;

    public class BetChoiceMatchingParam
    {
        protected BetChoiceMatchingParam(int custId, DateTime winlostDate, DateTime moveBetsDay, int betCount, bool isExportExcel)
        {
            CustId = custId;
            WinlostDate = winlostDate.Date;
            MoveBetsDay = moveBetsDay.Date;
            BetCount = betCount;
            IsExportExcel = isExportExcel;
        }

        public BetChoiceMatchingParam(int custId, DateTime winlostDate, DateTime moveBetsDay, int betCount)
        : this(custId, winlostDate, moveBetsDay, betCount, false)
        {
        }

        public int CustId { get; }

        public DateTime WinlostDate { get; }

        public DateTime MoveBetsDay { get; }

        public int BetCount { get; }

        public bool IsExportExcel { get; }
    }
}
