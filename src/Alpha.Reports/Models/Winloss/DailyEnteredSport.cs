namespace Alpha.Reports.Models.Winloss
{
    using System;

    public class DailyEnteredSport
    {
        public DailyEnteredSport()
        {
        }

        public DailyEnteredSport(int entered, int nonEntered)
        {
            Entered = entered;
            NonEntered = nonEntered;
        }

        public DailyEnteredSport(DateTime eventDate, int entered, int nonEntered)
        {
            EventDate = eventDate;
            Entered = entered;
            NonEntered = nonEntered;
        }

        public DailyEnteredSport(
            bool isToday,
            DateTime eventDate,
            int sportType,
            int entered,
            int nonEntered)
        {
            IsToday = isToday;
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