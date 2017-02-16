namespace Alpha.Reports.Criteria.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Models;

    public class WinLossCriteria : ReportProductCriteria
    {
        public WinLossCriteria(
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
                return "Age_Rpt_WinLoss_Member";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLoss_Agent";
            }

            return "Age_Rpt_WinLoss_Master";
        }
    }
}