namespace Alpha.Reports.Services
{
    using System;
    using Models;
    using Params;

    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();

        DateRange GetReportDateRange(ReportDateRangeParam param);
    }
}
