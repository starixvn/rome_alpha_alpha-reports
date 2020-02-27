namespace Alpha.Reports.Criteria.RiskAnalysisIPMatching
{
    using System;
    using Fanex.Data.Repository;

    public class RiskAnalysisIPMatchingCriteria : CriteriaBase
    {
        public RiskAnalysisIPMatchingCriteria(int custId, DateTime winlostDate, DateTime moveBetsDay, int memberCount)
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

        public override string GetSettingKey() => "Age_BetStatistics_IPMatching_Master";

        public override bool IsValid() => CustId > 0;
    }
}
