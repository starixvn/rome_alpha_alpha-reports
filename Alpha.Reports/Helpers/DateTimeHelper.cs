namespace Alpha.Reports.Helpers
{
    using System;

    public static class DateTimeHelper
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        private const string HtmlLongDateWithSlash = "MM/dd/yyyy<br/>hh:mm:ss tt";

        public static DateTime GetBeginningOfDate(DateTime dateTime)
        {
            var beginningDateTime = dateTime.ToShortDateString() + " 00:00:00 AM";

            return Convert.ToDateTime(beginningDateTime);
        }

        public static DateTime GetEndingOfDate(DateTime dateTime)
        {
            var endingDateTime = dateTime.ToShortDateString() + " 23:59:59 PM";

            return Convert.ToDateTime(endingDateTime);
        }

        public static string ToShortDateWithSlash(DateTime dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return dateTime.ToString(ShortDateWithSlash);
        }

        public static string ToLongDateWithSlash(DateTime dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return dateTime.ToString(LongDateWithSlash);
        }

        public static string ToHtmlLongDateWithSlash(DateTime dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return dateTime.ToString(HtmlLongDateWithSlash);
        }
    }
}
