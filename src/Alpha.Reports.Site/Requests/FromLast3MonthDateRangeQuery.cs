namespace Alpha.Reports.Site.Requests
{
    using System;
    using Extensions;
    using Reports.Models;
    using Utils;

    public class FromLast3MonthDateRangeQuery : ReportBaseQuery
    {
        internal FromLast3MonthDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate => SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate && FromDate >= MinDate;
    }
}