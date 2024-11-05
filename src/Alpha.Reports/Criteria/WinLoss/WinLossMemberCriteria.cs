namespace Alpha.Reports.Criteria.WinLoss
{
    using System;

    public class WinLossMemberCriteria : ReportProductCriteria
    {
        public WinLossMemberCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            string productDetails)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ProductDetails = productDetails;
        }

        public string ProductDetails { get; }

        public override string GetSettingKey() => "Age_Rpt_WinLoss_Member";
    }
}