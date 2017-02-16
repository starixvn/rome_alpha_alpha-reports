namespace Alpha.Reports.Site.Requests.Outstanding
{
    using Reports.Models;
    using ViewModels.Outstanding;
    using MediatR;

    public class OutstandingQuery : ReportBaseQuery, IRequest<OutstandingViewModel>
    {
        public OutstandingQuery(IUser user) : base(user)
        {
        }

        public string UserSelectedProductIds { get; set; }
    }
}