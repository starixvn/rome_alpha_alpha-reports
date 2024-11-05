using System;
using Alpha.Reports.Models;

namespace Alpha.Reports.Criteria.WinLossDetail
{
    public class WinLossDetailMasterCriteria : WinLossDetailAgentCriteria
    {
        public WinLossDetailMasterCriteria(
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
            : base(custId, pageIndex, pageSize, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, reportLevel, productDetails, username, isReward, subfix, levelType)
        {
        }

        public override string GetSettingKey() => $"Age_Rpt_WinLossDetail_Master{Subfix}";
    }
}
