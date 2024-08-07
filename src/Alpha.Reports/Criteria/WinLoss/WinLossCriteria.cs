namespace Alpha.Reports.Criteria.WinLoss
{
    using System;
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
            string productIds,
            UserLevel reportLevel,
            string productDetails)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
            ProductDetails = productDetails;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public string ProductDetails { get; }

        public override string GetSettingKey()
            => ReportLevel.IsAgentLevel()
            ? "Age_Rpt_WinLoss_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_WinLoss_Agent"
            : "Age_Rpt_WinLoss_Master";
    }
}