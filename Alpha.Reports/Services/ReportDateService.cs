namespace Alpha.Reports.Services
{
    using System;
    using Models;

    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();        
    }
}
