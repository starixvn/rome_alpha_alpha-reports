namespace Alpha.Reports.Site.Requests
{
    using System;
    using Extensions;
    using Reports.Models;
    using Services;

    public class ReportDefaultDateRangeQuery : ReportBaseQuery
    {
        internal ReportDefaultDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = DateTimeManager.Now;
            FromDate = DateTime.MinValue;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsHistoricReport { get; set; }

        public DateTime MinDate => IsHistoricReport
                ? DateTimeManager.Now.AddYears(-3).FirstDayOfMonth()
                : DateTimeManager.Now.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
        {
            //// TODO: Side effect here
            if (FromDate == DateTime.MinValue)
            {
                FromDate = IsHistoricReport
                    ? DateTimeManager.Now.FirstDayOfMonth()
                    : DateTimeManager.Now;
            }

            return base.IsValid()
                && FromDate.Date <= ToDate.Date
                && FromDate.Date >= MinDate.Date
                && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
        }
    }
}