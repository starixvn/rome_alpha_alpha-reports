namespace Alpha.Reports.Tests.TestFixtures
{
    using System;
    using Reports.Models;

    public class TestContextFixture : ApplicationContext
    {
        private DateTime currentDate = new DateTime(2016, 12, 2);

        public DateTime CurrentDate
        {
            get
            {
                return currentDate;
            }
        }
    }
}
