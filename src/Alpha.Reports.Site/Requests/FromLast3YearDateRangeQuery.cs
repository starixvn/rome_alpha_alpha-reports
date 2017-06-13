namespace Alpha.Reports.Site.Requests
{
    using System;
    using Extensions;
    using Reports.Models;
    using Utils;

    public class FromLast3YearDateRangeQuery : ReportBaseQuery
    {
        internal FromLast3YearDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate => SystemTime.Today.AddYears(-3).FirstDayOfYear();

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate && FromDate >= MinDate;
    }
}