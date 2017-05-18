namespace Alpha.Reports.Site.Requests.RacingCommission
{
    using MediatR;
    using Reports.Models;
    using ViewModels.RacingCommission;

    public class RacingCommissionQuery : ReportDefaultDateRangeQuery, IRequest<RacingCommissionViewModel>
    {
        public RacingCommissionQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedDateRangeOption { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsFilterVisible { get; set; } = true;
    }
}