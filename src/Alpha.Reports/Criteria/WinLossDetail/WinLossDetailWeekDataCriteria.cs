namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections.Generic;

    public class WinLossDetailWeekDataCriteria : ReportProductCriteria
    {
        public WinLossDetailWeekDataCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            string productDetails,
            string username)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ProductDetails = productDetails;
            Username = username;
        }

        public string ProductDetails { get; }

        public string Username { get; }

        public override string GetSettingKey() => "Age_Rpt_WinlossDetail_WeekDataSel";
    }
}
