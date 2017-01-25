namespace Alpha.Reports.Site.Requests
{
    using System;
    using Extensions;
    using Reports.Models;
    using Utils;

    public class ReportDefaultDateRangeQuery : ReportBaseQuery
    {
        internal ReportDefaultDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            FromDate = DateTime.MinValue;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsHistoricReport { get; set; }

        public DateTime MinDate => IsHistoricReport
                ? SystemTime.Today.AddYears(-3).FirstDayOfMonth()
                : SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
        {
            //// TODO: Side effect here
            if (FromDate == DateTime.MinValue)
            {
                FromDate = IsHistoricReport
                    ? SystemTime.Today.FirstDayOfMonth()
                    : SystemTime.Today;
            }

            return base.IsValid()
                && FromDate <= ToDate
                && FromDate >= MinDate
                && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
        }
    }
}