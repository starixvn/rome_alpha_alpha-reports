namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using Fanex.Data;

    public class WinLossDetailWeekDataCriteria : ReportProductCriteria
    {
        public WinLossDetailWeekDataCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            string productDetails,
            string username,
            bool isReward,
            byte levelType,
            string subfix) : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ProductDetails = productDetails;
            Username = username;
            IsReward = isReward;
            LevelType = levelType;
            Subfix = subfix;
        }

        public string ProductDetails { get; }

        public string Username { get; }

        public bool IsReward { get; }

        public byte LevelType { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey() => $"Age_Rpt_WinlossDetail_WeekDataSel{Subfix}";
    }
}
