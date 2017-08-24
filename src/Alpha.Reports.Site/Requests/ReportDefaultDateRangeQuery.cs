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
            ? SystemTime.Today.AddYears(-3).FirstDayOfYear()
            : SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
        {
            //// TODO: Side effect here
            AssignFromDateValue();

            return base.IsValid() && CheckDateRangeConstraint();
        }

        public void AssignFromDateValue()
        {
            if (FromDate == DateTime.MinValue)
            {
                FromDate = IsHistoricReport
                    ? SystemTime.Today.FirstDayOfMonth()
                    : SystemTime.Today;
            }
        }

        public bool CheckDateRangeConstraint()
            => FromDate <= ToDate
            && FromDate >= MinDate
            && (!IsHistoricReport || FromDate.IsSameMonthWith(ToDate));
    }
}