using System;

namespace Alpha.Reports.Criteria.CommissionByBettype
{
    public class CommissionByBettypeAgentCriteria : CommissionByBettypeMemberCriteria
    {
        public CommissionByBettypeAgentCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            byte levelType) : base(custId, fromDate, toDate, moveBetsDay)
        {
            LevelType = levelType;
        }

        public byte LevelType { get; }

        public override string GetSettingKey() => "Age_Rpt_CommByBettype_Agent";
    }
}
