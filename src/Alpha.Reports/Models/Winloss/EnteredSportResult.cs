namespace Alpha.Reports.Models.Winloss
{
    public class EnteredSportResult
    {
        public EnteredSportResult()
        {
        }

        public EnteredSportResult(
            int sportTypeId,
            string sportName,
            DailyEnteredSport enteredSportToday,
            DailyEnteredSport enteredSportYesterday)
        {
            SportTypeId = sportTypeId;
            SportName = sportName;
            EnteredSportToday = enteredSportToday;
            EnteredSportYesterday = enteredSportYesterday;
        }

        public int SportTypeId { get; }

        public string SportName { get; }

        public DailyEnteredSport EnteredSportToday { get; }

        public DailyEnteredSport EnteredSportYesterday { get; }
    }
}