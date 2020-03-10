namespace Alpha.Reports.Criteria.BetChoiceMatching
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class BetChoiceUplinesCriteria : CriteriaBase
    {
        public BetChoiceUplinesCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public override string GetSettingKey() => "Age_Customer_Upline_Sel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}
