namespace Alpha.Reports.Criteria.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Models;

    public class MatchWinLossDetailCriteria : ReportProductCriteria
    {
        public MatchWinLossDetailCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel reportLevel,
            string leagues) :
            base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
            Leagues = leagues;
        }

        [SpParam(SqlParamName = "League")]
        public string Leagues { get; }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override string GetSettingKey()
        {
            if (ReportLevel.IsAgentLevel())
            {
                return "Age_Rpt_MatchWinLossDetail_Agent";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "Age_Rpt_MatchWinLossDetail_Master";
            }

            return "Age_Rpt_MatchWinLossDetail_Super";
        }

        public override bool IsValid() =>
            CustId > 0
            && FromDate <= ToDate
            && !string.IsNullOrWhiteSpace(ProductIds)
            && !string.IsNullOrWhiteSpace(Leagues);
    }
}
