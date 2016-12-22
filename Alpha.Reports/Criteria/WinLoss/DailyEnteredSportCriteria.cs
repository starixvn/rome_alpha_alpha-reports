namespace Alpha.Reports.Criteria.WinLoss
{
    using Fanex.Data.Repository;

    public class DailyEnteredSportCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_Rpt_MatchSelResultEntered";
        }

        public override bool IsValid() => true;
    }
}
