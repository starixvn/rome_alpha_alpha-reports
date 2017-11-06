namespace Alpha.Reports.Tests.Constants
{
    using System;

    public static class DefaultVariablesTest
    {
        public const int PageSize = 1000;
        public const int PageIndex = 1;

        public static DateTime CurrentDate => new DateTime(2016, 12, 2);

        public static DateTime BalanceUpDay => new DateTime(2016, 11, 26);
    }
}
