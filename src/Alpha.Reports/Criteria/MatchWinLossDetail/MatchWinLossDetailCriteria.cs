namespace Alpha.Reports.Criteria.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class MatchWinLossDetailCriteria : CriteriaBase
    {
        public MatchWinLossDetailCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel reportLevel,
            IEnumerable<int> leagues)
        {
            ReportLevel = reportLevel;
            Leagues = leagues.JoinWithComma();

            ProductIds = productIds == null ? string.Empty : productIds.JoinWithComma();
            CustId = custId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            MoveBetsDay = moveBetsDay;
        }

        [SpParam(SqlParamName = "League")]
        public string Leagues { get; }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public string ProductIds { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey()
        {
            if (ReportLevel.IsAgentLevel())
            {
                return "Age_Rpt_MatchWinLossDetail_Member";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "Age_Rpt_MatchWinLossDetail_Agent";
            }

            return "Age_Rpt_MatchWinlossDetail_Master";
        }

        public override bool IsValid() =>
            CustId > 0
            && FromDate <= ToDate
            && !string.IsNullOrWhiteSpace(ProductIds)
            && !string.IsNullOrWhiteSpace(Leagues);
    }
}
