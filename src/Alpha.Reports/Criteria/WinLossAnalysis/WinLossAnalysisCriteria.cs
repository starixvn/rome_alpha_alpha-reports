namespace Alpha.Reports.Criteria.WinLossAnalysis
{
    using System;
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
            string productIds,
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
            => ReportLevel.IsAgentLevel()
            ? "Age_Rpt_WinlossAnalysis_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_WinlossAnalysis_Agent"
            : "Age_Rpt_WinlossAnalysis_Master";
    }
}