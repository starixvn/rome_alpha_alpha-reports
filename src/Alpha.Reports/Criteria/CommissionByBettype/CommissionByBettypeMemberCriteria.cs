namespace Alpha.Reports.Criteria.CommissionByBettype
{
    using System;
    using Fanex.Data.Repository;

    public class CommissionByBettypeMemberCriteria : CriteriaBase
    {
        public CommissionByBettypeMemberCriteria(int custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => "Age_Rpt_CommByBettype_Member";

        public override bool IsValid() => CustId > 0;
    }
}
