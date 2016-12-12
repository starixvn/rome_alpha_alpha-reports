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
            EnteredSportDaily enterSportToday,
            EnteredSportDaily enterSportYesterday)
        {
            SportTypeId = sportTypeId;
            SportName = sportName;
            EnterSportToday = enterSportToday;
            EnterSportYesterday = enterSportYesterday;
        }

        public int SportTypeId { get; }

        public string SportName { get; }

        public EnteredSportDaily EnterSportToday { get; }

        public EnteredSportDaily EnterSportYesterday { get; }
    }
}