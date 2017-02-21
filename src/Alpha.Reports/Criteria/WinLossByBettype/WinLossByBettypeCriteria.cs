namespace Alpha.Reports.Criteria.WinLossByBetTypeCriteria
{
    using System;
    using Fanex.Data;
    using Models;

    public class WinLossByBetTypeCriteria : ReportCriteriaBase
    {
        public WinLossByBetTypeCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            UserLevel custLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLossByBettype_Member";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLossByBettype_Agent";
            }

            return "Age_Rpt_WinLossByBettype_Master";
        }
    }
}
