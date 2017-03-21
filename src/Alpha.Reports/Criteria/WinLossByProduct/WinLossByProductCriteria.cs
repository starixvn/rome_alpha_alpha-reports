namespace Alpha.Reports.Criteria.WinLossByProductCriteria
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Models;

    public class WinLossByProductCriteria : ReportProductCriteria
    {
        public WinLossByProductCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel custLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLossByProduct_Member";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLossByProduct_Agent";
            }

            return "Age_Rpt_WinLossByProduct_Master";
        }
    }
}
