namespace Alpha.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class TimeToArrangeBetsInDatabaseCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_SelBalanceUpDay";

        public override bool IsValid() => true;
    }
}
