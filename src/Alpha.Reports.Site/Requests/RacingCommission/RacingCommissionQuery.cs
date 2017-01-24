namespace Alpha.Reports.Site.Requests.RacingCommission
{
    using System;
    using MediatR;
    using Reports.Models;
    using ViewModels.RacingCommission;

    public class RacingCommissionQuery : ReportBaseQuery, IRequest<RacingCommissionViewModel>
    {
        public RacingCommissionQuery(IUser viewer) : base(viewer)
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }

        public int UserSelectedDateRangeOption { get; set; }
    }
}