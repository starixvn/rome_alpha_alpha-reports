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
            => CustLevel.IsAgentLevel()
            ? "Age_Rpt_CommByBettype_HR_Member"
            : CustLevel.IsMasterLevel()
            ? "Age_Rpt_CommByBettype_HR_Agent"
            : "Age_Rpt_CommByBettype_HR_Master";

        public override bool IsValid()
            => !CustLevel.IsMemberLevel();
    }
}
