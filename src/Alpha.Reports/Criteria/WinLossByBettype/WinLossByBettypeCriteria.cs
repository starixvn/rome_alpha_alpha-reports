namespace Alpha.Reports.Criteria.WinLoss
{
    using System;
    using Fanex.Data;
    using Models;

    public class WinLossByBettypeCriteria : ReportCriteriaBase
    {
        public WinLossByBettypeCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            UserLevel custLevel) : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLossByBettype_Ag";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLossByBettype_MAg";
            }

            return "Age_Rpt_WinLossByBettype_SAg";
        }
    }
}
