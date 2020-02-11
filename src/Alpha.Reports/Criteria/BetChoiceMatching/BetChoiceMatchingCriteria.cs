namespace Alpha.Reports.Criteria.BetChoiceMatching
{
    using Fanex.Data.Repository;
    using System;

    public class BetChoiceMatchingCriteria : CriteriaBase
    {
        public BetChoiceMatchingCriteria(int custId, DateTime winlostDate, DateTime moveBetsDay, int betCount, bool isExportExcel)
        {
            CustId = custId;
            WinlostDate = winlostDate.Date;
            MoveBetsDay = moveBetsDay.Date;
            BetCount = betCount;
            IsExportExcel = isExportExcel;
        }

        public int CustId { get; }

        public DateTime WinlostDate { get; }

        public DateTime MoveBetsDay { get; }

        public int BetCount { get; }

        public bool IsExportExcel { get; }

        public override string GetSettingKey() => "Age_BetStatistics_BetMatching_Master";

        public override bool IsValid() => CustId > 0;
    }
}
