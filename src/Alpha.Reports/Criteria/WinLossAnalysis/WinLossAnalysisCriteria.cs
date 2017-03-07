namespace Alpha.Reports.Criteria.WinLossAnalysis
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Models;

    public class WinLossAnalysisCriteria : ReportProductCriteria
    {
        public WinLossAnalysisCriteria(
            int custId,
            int reportType,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel reportLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
            ReportType = reportType;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public int ReportType { get; }

        public override string GetSettingKey()
        {
            if (ReportLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinlossAnalysis_Member";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinlossAnalysis_Agent";
            }

            return "Age_Rpt_WinlossAnalysis_Master";
        }
    }
}