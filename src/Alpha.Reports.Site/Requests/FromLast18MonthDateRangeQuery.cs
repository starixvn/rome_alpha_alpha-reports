namespace Alpha.Reports.Site.Requests
{
    using System;
    using Extensions;
    using Reports.Models;
    using Utils;

    public class FromLast18MonthDateRangeQuery : ReportBaseQuery
    {
        internal FromLast18MonthDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate => SystemTime.Today.AddMonths(-18).FirstDayOfMonth();

        public override bool IsValid()
        {
            return base.IsValid()
                && MinDate <= FromDate
                && FromDate <= ToDate;
        }
    }
}