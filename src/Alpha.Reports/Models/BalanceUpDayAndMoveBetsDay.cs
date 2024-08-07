namespace Alpha.Reports.Models
{
    using System;

    public class BalanceUpDayAndMoveBetsDay
    {
        public BalanceUpDayAndMoveBetsDay()
        {
        }

        public BalanceUpDayAndMoveBetsDay(DateTime balanceUpDay, DateTime moveBetsDay)
        {
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}