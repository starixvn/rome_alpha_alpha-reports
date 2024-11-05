namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using Fanex.Data;
    using Models;

    public class WinLossDetailMemberCriteria : PagingReportProductCriteria
    {
        public WinLossDetailMemberCriteria(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            UserLevel reportLevel,
            string productDetails,
            string username,
            bool isReward,
            string subfix)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
            ProductDetails = productDetails;
            Username = username;
            IsReward = isReward;
            Subfix = subfix;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public string ProductDetails { get; }

        public string Username { get; }

        public bool IsReward { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey() => $"Age_Rpt_WinLossDetail_Member{Subfix}";
    }
}