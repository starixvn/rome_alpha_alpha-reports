namespace Alpha.Reports.Criteria.BetChoiceMatching
{
    using Fanex.Data.Repository;
    using System;

    public class BetChoiceMatchingCriteria : CriteriaBase
    {
        public BetChoiceMatchingCriteria(int custId, DateTime winlostDate, DateTime moveBetsDay, int betCount)
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

        public override string GetSettingKey() => "Age_BetStatistics_BetMatching_Master";

        public override bool IsValid() => CustId > 0;
    }
}
