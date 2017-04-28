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
            => ReportLevel.IsAgentLevel()
            ? "Age_Rpt_WinlossAnalysisChart_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_WinlossAnalysisChart_Agent"
            : "Age_Rpt_WinlossAnalysisChart_Master";
    }
}