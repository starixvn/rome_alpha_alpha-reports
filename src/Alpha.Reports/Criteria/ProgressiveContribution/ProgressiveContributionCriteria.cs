namespace Alpha.Reports.Criteria.ProgressiveContribution
{
    using System;
    using Fanex.Data;
    using Models;

    public class ProgressiveContributionCriteria : ReportProductCriteria
    {
        public ProgressiveContributionCriteria(
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
            ? "Age_Rpt_ProgressiveGamesContribution_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_ProgressiveGamesContribution_Agent"
            : "Age_Rpt_ProgressiveGamesContribution_Master";
    }
}