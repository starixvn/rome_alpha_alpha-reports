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
            ToDate = DateTimeUtility.Today;
            FromDate = DateTime.MinValue;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsHistoricReport { get; set; }

        public DateTime MinDate => IsHistoricReport
                ? DateTimeUtility.Today.AddYears(-3).FirstDayOfMonth()
                : DateTimeUtility.Today.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
        {
            //// TODO: Side effect here
            if (FromDate == DateTime.MinValue)
            {
                FromDate = IsHistoricReport
                    ? DateTimeUtility.Today.FirstDayOfMonth()
                    : DateTimeUtility.Today;
            }

            return base.IsValid()
                && FromDate <= ToDate
                && FromDate >= MinDate
                && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
        }
    }
}