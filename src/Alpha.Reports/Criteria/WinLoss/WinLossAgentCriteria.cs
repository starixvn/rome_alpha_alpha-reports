using System;

namespace Alpha.Reports.Criteria.WinLoss
{
    public class WinLossAgentCriteria : WinLossMemberCriteria
    {
        public WinLossAgentCriteria(
           int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            string productDetails,
            byte levelType)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails)
        {
            LevelType = levelType;
        }

        public byte LevelType { get; }

        public override string GetSettingKey() => "Age_Rpt_WinLoss_Agent";
    }
}
