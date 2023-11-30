namespace Alpha.Reports.Criteria.WinLossAnalysisChart
{
    using System;
    using Fanex.Data;
    using Models;

    public class WinLossAnalysisChartCriteria : ReportProductCriteria
    {
        public WinLossAnalysisChartCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,            
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            UserLevel reportLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;            
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override string GetSettingKey()
            => ReportLevel.IsAgentLevel()
            ? "Age_Chart_WinlossAnalysis_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Chart_WinlossAnalysis_Agent"
            : "Age_Chart_WinlossAnalysis_Master";        
    }
}