namespace Alpha.Reports.Site.Tests.Constants
{
    using System;

    public static class DefaultVariablesTest
    {
        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const int AllCurrencies = 0;

        public static DateTime CurrentDate => new DateTime(2016, 12, 2);

        public static DateTime BalanceUpDate => new DateTime(2016, 11, 28);

        public static DateTime MoveBetsDay => new DateTime(2016, 11, 29);
    }
}