namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Models;

    public class WinLossDetailCriteria : PagingReportProductCriteria
    {
        public WinLossDetailCriteria(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel reportLevel,
            string productDetails,
            string username,
            string subfix)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
            ProductDetails = productDetails;
            Username = username;
            Subfix = subfix;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public string ProductDetails { get; }

        public string Username { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey()
            => ReportLevel.IsAgentLevel()
            ? $"Age_Rpt_WinLossDetail_Member{Subfix}"
            : ReportLevel.IsMasterLevel()
            ? $"Age_Rpt_WinLossDetail_Agent{Subfix}"
            : $"Age_Rpt_WinLossDetail_Master{Subfix}";
    }
}