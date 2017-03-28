namespace Alpha.Reports.Criteria.MatchWinLoss
{
    using System;
    using Fanex.Data.Repository;

    public class GetMatchWinLossCriteria : CriteriaBase
    {
        public GetMatchWinLossCriteria(int custId, DateTime fromDate, DateTime toDate)
        {
            CustId = custId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public override string GetSettingKey() => "Age_Rpt_MatchWinLoss_Member";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}
