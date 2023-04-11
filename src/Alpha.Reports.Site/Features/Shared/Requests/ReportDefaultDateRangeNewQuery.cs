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
            HistoricPivotDate = today.AddMonths(NegativeThree).FirstDayOfMonth();
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime HistoricPivotDate { get; }

        public override bool IsValid()
            => base.IsValid() && CheckDateRangeConstraint();

        public bool CheckDateRangeConstraint()
        {
            if (FromDate <= ToDate && MinDate <= FromDate)
            {
                var historicPivotDate = HistoricPivotDate;
                const int maxDaysInOneMonth = 31;

                return !(
                    ToDate < historicPivotDate && ToDate.Subtract(FromDate).TotalDays >= maxDaysInOneMonth
                    || FromDate < historicPivotDate && historicPivotDate <= ToDate
                );
            }

            return false;
        }
    }
}