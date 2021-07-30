namespace Alpha.Reports.Features.SuperWinLossDetailByPT.Criteria
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Criteria;

    public class SuperWinLossDetailByPTCriteria : ReportProductCriteria
    {
        public SuperWinLossDetailByPTCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            string productDetails)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ProductDetails = productDetails;
        }

        public string ProductDetails { get; }

        public override string GetSettingKey() => "Age_Rpt_WinLossDetailByPT_Super";
    }
}