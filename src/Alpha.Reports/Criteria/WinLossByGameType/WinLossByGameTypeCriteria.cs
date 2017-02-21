namespace Alpha.Reports.Criteria.WinLossByGameType
{
    using System;
    using Fanex.Data;
    using Models;

    public class WinLossByGameTypeCriteria : ReportCriteriaBase
    {
        public WinLossByGameTypeCriteria(
            int custId,
            int productId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            UserLevel custLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay)
        {
            CustLevel = custLevel;
            ProductId = productId;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int ProductId { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLossByGameType_Member";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLossByGameType_Agent";
            }

            return "Age_Rpt_WinLossByGameType_Master";
        }

        public override bool IsValid() => base.IsValid() && FromDate <= ToDate;
    }
}
