namespace Alpha.Reports.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        private const string HtmlLongDateWithSlash = "MM/dd/yyyy<br/>hh:mm:ss tt";

        public static DateTime GetBeginningOfDate(this DateTime dateTime)
        {
            var beginningDateTime = dateTime.ToShortDateString() + " 00:00:00 AM";

            return Convert.ToDateTime(beginningDateTime);
        }

        public static DateTime GetEndingOfDate(this DateTime dateTime)
        {
            var endingDateTime = dateTime.ToShortDateString() + " 23:59:59 PM";

            return Convert.ToDateTime(endingDateTime);
        }

        public static string ToShortDateWithSlash(this DateTime dateTime)
        {
            return dateTime.ToString(ShortDateWithSlash);
        }

        public static string ToLongDateWithSlash(this DateTime dateTime)
        {
            return dateTime.ToString(LongDateWithSlash);
        }

        public static string ToHtmlLongDateWithSlash(this DateTime dateTime)
        {
            return dateTime.ToString(HtmlLongDateWithSlash);
        }

        public static DateTime ConvertGMT(this DateTime dateTime, int gmt)
        {
            var hoursOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime).Hours;
            DateTime gmtDateTime = dateTime.AddHours(gmt - hoursOffset);

            return gmtDateTime;
        }
    }
}
