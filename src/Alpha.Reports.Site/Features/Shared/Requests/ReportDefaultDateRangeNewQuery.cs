namespace Alpha.Reports.Site.Features.Shared.Requests
{
    using System;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Utils;

    public class ReportDefaultDateRangeNewQuery : ReportBaseQuery
    {
        private const int NegativeThree = -3;

        protected ReportDefaultDateRangeNewQuery(IUser viewer) : base(viewer)
        {
            MinDate = SystemTime.Today.AddMonths(NegativeThree)
                .FirstDayOfMonth()
                .GetGreaterDate(LimitedDateToShowInfo);
            FromDate = GetFromDate();
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; } = SystemTime.Today;

        public DateTime HistoricMinDate
            => SystemTime.Today.AddYears(NegativeThree).FirstDayOfYear().GetGreaterDate(LimitedDateToShowInfo);

        public override bool IsValid()
            => base.IsValid() && CheckDateRangeConstraint();

        public bool CheckDateRangeConstraint()
            => FromDate <= ToDate
            && FromDate >= MinDate
            && (!IsHistoricReport || ToDate.Subtract(FromDate).TotalDays <= 31);

        private DateTime GetFromDate()
        {
            var fromDate = IsHistoricReport ? SystemTime.Today.FirstDayOfMonth() : SystemTime.Today;

            return fromDate.GetGreaterDate(LimitedDateToShowInfo);
        }
    }
}