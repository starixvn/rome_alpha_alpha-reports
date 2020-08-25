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
            UserLevel custLevel,
            string productDetails)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            CustLevel = custLevel;
            ProductDetails = productDetails;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public string ProductDetails { get; }

        public override string GetSettingKey()
            => CustLevel.IsAgentLevel()
            ? "Age_Rpt_WinLossByProduct_Member"
            : CustLevel.IsMasterLevel()
            ? "Age_Rpt_WinLossByProduct_Agent"
            : "Age_Rpt_WinLossByProduct_Master";
    }
}
