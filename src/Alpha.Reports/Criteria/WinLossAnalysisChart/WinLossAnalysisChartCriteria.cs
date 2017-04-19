namespace Alpha.Reports.Criteria.WinLossAnalysisChart
{
    using Fanex.Data;
    using Models;
    using System;
    using System.Collections.Generic;

    public class WinLossAnalysisChartCriteria : ReportProductCriteria
    {
        public WinLossAnalysisChartCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel reportLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override string GetSettingKey()
        {
            if (ReportLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinlossAnalysisChart_Member";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinlossAnalysisChart_Agent";
            }

            return "Age_Rpt_WinlossAnalysisChart_Master";
        }
    }
}