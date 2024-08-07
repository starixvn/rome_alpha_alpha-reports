namespace Alpha.Reports.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetLeaguesByCustomerCriteria : CriteriaBase
    {
        public GetLeaguesByCustomerCriteria(
            int custId,
            string productIds,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            string language)
        {
            CustId = custId;
            ProductIds = productIds;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
            Language = language;
        }

        public int CustId { get; }

        public string ProductIds { get; }

        [SpParam(SqlParamName = "IsShowAll")]
        public bool IncludeTestLeagues => true;

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Age_Rpt_MatchWinloss_GetLeagueByProduct";

        public override bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(ProductIds)
            && FromDate <= ToDate;
    }
}
