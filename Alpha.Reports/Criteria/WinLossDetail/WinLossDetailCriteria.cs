namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections.Generic;
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
            IEnumerable<int> productIdList,
            UserLevel userLevel)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay, productIdList)
        {
            ViewerLevel = userLevel;
        }

        private UserLevel ViewerLevel { get; }

        public override string GetSettingKey()
        {
            if (ViewerLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLossDetail_Member";
            }

            if (ViewerLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLossDetail_Agent";
            }

            return "Age_Rpt_WinLossDetail_Master";
        }
    }
}
