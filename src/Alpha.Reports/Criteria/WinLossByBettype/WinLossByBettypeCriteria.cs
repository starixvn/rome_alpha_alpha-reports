namespace Alpha.Reports.Criteria.WinLoss
{
    using System;
    using Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class WinLossByBettypeCriteria : CriteriaBase
    {
        public WinLossByBettypeCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            UserLevel custLevel)
        {
            CustId = custId;
            FromDate = fromDate.GetBeginningOfDate();
            ToDate = toDate.GetBeginningOfDate();
            BalanceUpDay = balanceUpDay;
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDay { get; }

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

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;

        public bool IsNotValid() => !IsValid();
    }
}
