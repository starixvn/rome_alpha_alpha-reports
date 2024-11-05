namespace Alpha.Reports.Site.Requests.CommissionByBettype
{
    using System;
    using MediatR;
    using Reports.Models;
    using Utils;
    using ViewModels.CommissionByBettype;

    public class CommissionByBettypeQuery : ReportDefaultDateRangeQuery, IRequest<CommissionByBettypeViewModel>
    {
        public CommissionByBettypeQuery(IUser viewer) : base(viewer)
        {
            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today;
        }

        public int UserSelectedDateRangeOption { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public byte LevelType { get; set; }
    }
}