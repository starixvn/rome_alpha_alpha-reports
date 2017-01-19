namespace Alpha.Reports.Criteria.RacingCommission
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class RacingCommissionCriteria : CriteriaBase
    {
        public RacingCommissionCriteria(UserLevel custLevel, int custId, DateTime fromDate, DateTime toDate)
        {
            CustLevel = custLevel;
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsSuperLevel())
            {
                return "Age_Rpt_RacingCommission_Master";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_RacingCommission_Agent";
            }

            return "Age_Rpt_RacingCommission_Member";
        }

        public override bool IsValid()
        {
            return !CustLevel.IsMemberLevel();
        }
    }
}
