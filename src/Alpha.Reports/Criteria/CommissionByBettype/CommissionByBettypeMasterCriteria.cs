using System;

namespace Alpha.Reports.Criteria.CommissionByBettype
{
    public class CommissionByBettypeMasterCriteria : CommissionByBettypeAgentCriteria
    {
        public CommissionByBettypeMasterCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            byte levelType) : base(custId, fromDate, toDate, moveBetsDay, levelType)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_CommByBettype_Master";
    }
}
