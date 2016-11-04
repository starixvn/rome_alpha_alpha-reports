namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using Models;

    public class WinLossDetailCriteria : ReportProductCriteria
    {
        public WinLossDetailCriteria(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            UserLevel reportLevel)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
        }

        private UserLevel ReportLevel { get; }      

        public override string GetSettingKey()
        {
            if (ReportLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLossDetail_Member";
            }

            if (ReportLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLossDetail_Agent";
            }

            return "Age_Rpt_WinLossDetail_Master";
        }
    }
}
