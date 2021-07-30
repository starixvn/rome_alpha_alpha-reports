namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT.Requests
{
    using Alpha.Reports.Site.Features.SuperWinLossDetailByPT.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;
    using Reports.Models;

    public class SuperWinLossDetailByPTQuery
        : FromLast3MonthDateRangeQuery, IRequest<SuperWinLossDetailByPTViewModel>
    {
        public SuperWinLossDetailByPTQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public string UserSelectedResourceIds { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public string SortPropertyName { get; set; } = "Turnover";

        public bool IsDescendingSort { get; set; } = true;

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}