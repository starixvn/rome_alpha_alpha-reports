namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Utils;
    using MediatR;
    using System;

    public class MoveStatementHistoryQuery : ReportBaseQuery, IRequest<MoveStatementHistoryViewModel>
    {
        private readonly DateTime ReleasedDateTime = new DateTime(2022, 5, 31);

        public MoveStatementHistoryQuery(IUser viewer) : base(viewer)
        {
            IsProtectedMode = viewer.IsProtectedMode;
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
            MinDate = ReleasedDateTime.GetGreaterDate(SystemTime.Today.AddMonths(-3).FirstDayOfMonth());
        }

        public string Username { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public bool IsProtectedMode { get; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate { get; set; }

        public override bool IsValid()
            => base.IsValid() 
            && FromDate <= ToDate 
            && FromDate >= MinDate;
    }
}