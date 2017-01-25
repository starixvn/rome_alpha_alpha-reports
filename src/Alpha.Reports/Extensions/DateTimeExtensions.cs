namespace Alpha.Reports.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";

        public static DateTime GetBeginningOfDate(this DateTime dateTime)
            => Convert.ToDateTime(dateTime.ToShortDateString() + " 00:00:00 AM");

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);

        public static DateTime FirstDayOfYear(this DateTime dateTime)
            => new DateTime(dateTime.Year, 1, 1);

        public static bool IsInOneMonth(this DateTime dateTime, DateTime otherDateTime)
            => dateTime.Month == otherDateTime.Month && dateTime.Year == otherDateTime.Year;
    }
}