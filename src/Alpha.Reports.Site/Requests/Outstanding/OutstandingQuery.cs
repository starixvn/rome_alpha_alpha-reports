using Alpha.Reports.Site.ViewModels.Outstanding;
using MediatR;

namespace Alpha.Reports.Site.Requests.Outstanding
{
    public class OutstandingQuery : IRequest<OutstandingViewModel>
    {
        public byte UserLevelId { get; set; }

        public byte CustLevelId { get; set; }

        public int CustId { get; set; }
    }
}