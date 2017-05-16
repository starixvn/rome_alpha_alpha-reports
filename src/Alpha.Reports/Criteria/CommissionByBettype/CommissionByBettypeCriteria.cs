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
            => CustLevel.IsAgentLevel()
            ? "Age_Rpt_CommByBettype_Member"
            : CustLevel.IsMasterLevel()
            ? "Age_Rpt_CommByBettype_Agent"
            : "Age_Rpt_CommByBettype_Master";

        public override bool IsValid()
            => !CustLevel.IsMemberLevel();
    }
}
