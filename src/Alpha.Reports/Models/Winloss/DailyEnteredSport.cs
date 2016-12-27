namespace Alpha.Reports.Models.Winloss
{
    using System;

    public class DailyEnteredSport
    {
        public DailyEnteredSport(
            int isToday,
            DateTime eventDate,
            int sportType,
            int entered,
            int nonEntered)
        {
            IsToday = !isToday.Equals(0);
            EventDate = eventDate;
            SportType = sportType;
            Entered = entered;
            NonEntered = nonEntered;
        }

        public bool IsToday { get; }

        public DateTime EventDate { get; }

        public int SportType { get; }

        public int Entered { get; }

        public int NonEntered { get; }
    }
}