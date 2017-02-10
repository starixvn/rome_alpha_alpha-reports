namespace Alpha.Reports.Criteria.WinLoss
{
    using System.Configuration;
    using Fanex.Data.Repository;

    public class DailyEnteredSportCriteria : CriteriaBase
    {
        public bool IsShowAll => ConfigurationManager.AppSettings["LocalTest"].Equals("1");

        public override string GetSettingKey() => "Age_Rpt_MatchSelResultEntered";

        public override bool IsValid() => true;
    }
}