namespace Alpha.Reports.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params;

    public class GetLeaguesByCustomerCriteria : CriteriaBase
    {
        public GetLeaguesByCustomerCriteria(
            GetLeaguesByCustomerParam getLeaguesByCustomerParam,
            DateTime moveBetsDay)
        {
            if (getLeaguesByCustomerParam == null)
            {
                throw new ArgumentNullException(nameof(getLeaguesByCustomerParam));
            }

            CustId = getLeaguesByCustomerParam.CustId;
            Products = getLeaguesByCustomerParam.Products;
            FromDate = getLeaguesByCustomerParam.FromDate;
            ToDate = getLeaguesByCustomerParam.ToDate;
            Language = getLeaguesByCustomerParam.Language;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public string Products { get; }

        [SpParam(SqlParamName = "IsShowAll")]
        public bool IncludeTestLeagues => true;

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Age_Rpt_MatchWinloss_SelLeagueBySport";

        public override bool IsValid() =>
            CustId > 0 &&
            !string.IsNullOrWhiteSpace(Products) &&
            FromDate <= ToDate;
    }
}
