namespace Alpha.Reports.Models.Winloss
{
    using System;

    public class EnteredSportDaily
    {
        public EnteredSportDaily()
        {
        }

        public EnteredSportDaily(
            bool isToday,
            DateTime eventDate,
            int sportTypeId,
            int entered,
            int nonEntered)
        {
            IsToday = IsToday;
            EventDate = eventDate;
            SportTypeId = sportTypeId;
            Entered = entered;
            NonEntered = nonEntered;
        }

        public bool IsToday { get; }

        public DateTime EventDate { get; }

        public int SportTypeId { get; }

        public int Entered { get; }

        public int NonEntered { get; }
    }
}