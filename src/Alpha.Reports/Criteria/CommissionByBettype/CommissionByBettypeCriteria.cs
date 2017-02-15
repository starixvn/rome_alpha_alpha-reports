namespace Alpha.Reports.Criteria.CommissionByBettype
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class CommissionByBettypeCriteria : CriteriaBase
    {
        public CommissionByBettypeCriteria(UserLevel custLevel, int custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay)
        {
            CustLevel = custLevel;
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey()
        {
            if (CustLevel.IsSuperLevel())
            {
                return "Age_Rpt_CommByBettype_Master";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_CommByBettype_Agent";
            }

            return "Age_Rpt_CommByBettype_Member";
        }

        public override bool IsValid()
        {
            return !CustLevel.IsMemberLevel();
        }
    }
}
