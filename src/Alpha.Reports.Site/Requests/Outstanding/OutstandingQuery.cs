using Alpha.Reports.Models;
using Alpha.Reports.Site.ViewModels.Outstanding;
using MediatR;

namespace Alpha.Reports.Site.Requests.Outstanding
{
    public class OutstandingQuery : ReportBaseQuery, IRequest<OutstandingViewModel>
    {
        public OutstandingQuery(IUser user) : base(user)
        {
        }
    }
}