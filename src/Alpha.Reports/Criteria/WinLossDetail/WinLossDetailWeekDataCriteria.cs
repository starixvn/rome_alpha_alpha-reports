namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;

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
            string username,
            string subfix)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ProductDetails = productDetails;
            Username = username;
            Subfix = subfix;
        }

        public string ProductDetails { get; }

        public string Username { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey() => $"Age_Rpt_WinlossDetail_WeekDataSel{Subfix}";
    }
}
