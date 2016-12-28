namespace Alpha.Reports.Services
{
    using System;

    public static class DateTimeManager
    {
        [ThreadStatic]
        private static DateTime? now;

        public static DateTime Now
        {
            get { return now ?? DateTime.Now; }
            set { now = value; }
        }
    }
}
