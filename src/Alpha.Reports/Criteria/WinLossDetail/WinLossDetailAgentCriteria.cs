using System;
using Alpha.Reports.Models;

namespace Alpha.Reports.Criteria.WinLossDetail
{
    public class WinLossDetailAgentCriteria : WinLossDetailMemberCriteria
    {
        public WinLossDetailAgentCriteria(
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
            string subfix,
            byte levelType)
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, reportLevel, productDetails, username, isReward, subfix)
        {
            LevelType = levelType;
        }

        public byte LevelType { get; }

        public override string GetSettingKey() => $"Age_Rpt_WinLossDetail_Agent{Subfix}";
    }
}
