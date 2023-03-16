namespace Alpha.Reports.Criteria.WinLoss
{
    using Alpha.Reports.Configurations;
    using Fanex.Data.Repository;

    public class DailyEnteredSportCriteria : CriteriaBase
    {
        public bool IncludeTestData { get; } = SiteSettings.LocalTest;

        public override string GetSettingKey() => "Age_Rpt_MatchSelResultEntered";

        public override bool IsValid() => true;
    }
}