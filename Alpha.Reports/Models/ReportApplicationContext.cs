namespace Alpha.Reports.Models
{
    using System;

    public class ReportApplicationContext : ApplicationContext
    {
        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}