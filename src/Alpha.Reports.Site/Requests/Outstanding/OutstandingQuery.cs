namespace Alpha.Reports.Site.Requests.Outstanding
{
    using MediatR;
    using Reports.Models;
    using ViewModels.Outstanding;

    public class OutstandingQuery : ReportBaseQuery, IRequest<OutstandingViewModel>
    {
        public OutstandingQuery(IUser user) : base(user)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }
    }
}