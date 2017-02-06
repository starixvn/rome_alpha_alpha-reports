namespace Alpha.Reports.Criteria.WinLossByGameType
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class WinLossByGameTypeCriteria : CriteriaBase
    {
        public WinLossByGameTypeCriteria(
            int custId,
            int productId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            UserLevel custLevel)
        {
            CustId = custId;
            ProductId = productId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            BalanceUpDay = balanceUpDay;
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public int ProductId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDay { get; }

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

        public override bool IsValid() => CustId > 0 && ProductId > 0 && FromDate <= ToDate;

        public bool IsNotValid() => !IsValid();
    }
}
