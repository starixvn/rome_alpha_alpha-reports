namespace Alpha.Reports.Site.Requests.CommissionByBettype
{
    using System;
    using MediatR;
    using Reports.Models;
    using ViewModels.CommissionByBettype;

    public class CommissionByBettypeQuery : ReportDefaultDateRangeQuery, IRequest<CommissionByBettypeViewModel>
    {
        public CommissionByBettypeQuery(IUser viewer) : base(viewer)
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }

        public int UserSelectedDateRangeOption { get; set; }
    }
}