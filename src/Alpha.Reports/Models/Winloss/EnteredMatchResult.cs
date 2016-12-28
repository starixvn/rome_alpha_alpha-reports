namespace Alpha.Reports.Models.Winloss
{
    public class EnteredMatchResult
    {
        public EnteredMatchResult(
            int sportTypeId,
            string sportName,
            DailyEnteredSport enterSportToday,
            DailyEnteredSport enterSportYesterday)
        {
            SportTypeId = sportTypeId;
            SportName = sportName;
            EnterSportToday = enterSportToday;
            EnterSportYesterday = enterSportYesterday;
        }

        public int SportTypeId { get; }

        public string SportName { get; }

        public DailyEnteredSport EnterSportToday { get; }

        public DailyEnteredSport EnterSportYesterday { get; }
    }
}