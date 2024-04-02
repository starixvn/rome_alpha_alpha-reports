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
            var today = SystemTime.Today;
            FromDate = today;
            ToDate = today;
            MinDate = today
                .AddYears(NegativeThree)
                .FirstDayOfYear()
                .GetGreaterDate(LimitedDateToShowInfo);
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override bool IsValid()
            => base.IsValid() && MinDate <= FromDate && FromDate <= ToDate;
    }
}