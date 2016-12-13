namespace Alpha.Reports.Services
{
    using System;

    public static class DateTimeAdapter
    {
        private static DateTime? _now;

        public static DateTime Now
        {
            get { return _now ?? DateTime.Now; }
            set { _now = value; }
        }
    }
}
