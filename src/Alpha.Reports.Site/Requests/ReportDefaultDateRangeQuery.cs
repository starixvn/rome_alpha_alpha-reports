namespace Alpha.Reports.Site.Requests
{
    using System;
    using Extensions;
    using Reports.Models;
    using Utils;

    public class ReportDefaultDateRangeQuery : ReportBaseQuery
    {
        private DateTime fromDate;

        internal ReportDefaultDateRangeQuery(IUser viewer) : base(viewer)
        {
            ToDate = SystemTime.Today;
            fromDate = DateTime.MinValue;
        }

        public DateTime FromDate
        {
            get
            {
                // Set data here because default value of from date depends on whether report is hictoric or not.
                if (fromDate == DateTime.MinValue)
                {
                    fromDate = IsHistoricReport
                        ? SystemTime.Today.FirstDayOfMonth()
                        : SystemTime.Today;
                }

                return fromDate;
            }

            set
            {
                fromDate = value;
            }
        }

        public DateTime ToDate { get; set; }

        public bool IsHistoricReport { get; set; }

        public DateTime MinDate => IsHistoricReport
                ? SystemTime.Today.AddYears(-3).FirstDayOfYear()
                : SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
        {
            return base.IsValid()
                && FromDate <= ToDate
                && FromDate >= MinDate
                && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
        }
    }
}