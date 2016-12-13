namespace Alpha.Reports.Tests.Constants
{
    using System;

    public static class DefaultVariables
    {
        public const int PageSize = 1000;
        public const int PageIndex = 1;

        public static DateTime CurrentDate
        {
            get
            {
                return new DateTime(2016, 12, 2);
            }
        }
    }
}
