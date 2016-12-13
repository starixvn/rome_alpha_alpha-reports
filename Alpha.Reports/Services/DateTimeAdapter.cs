namespace Alpha.Reports.Services
{
    using System;

    public static class DateTimeAdapter
    {
        private static DateTime? now;

        public static DateTime Now
        {
            get { return now ?? DateTime.Now; }
            set { now = value; }
        }
    }
}
