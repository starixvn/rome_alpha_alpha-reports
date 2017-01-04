using System;
using Alpha.Reports.Site.Requests.Outstanding;
using Alpha.Reports.Site.ViewModels.Outstanding;
using MediatR;

namespace Alpha.Reports.Site.Handlers.Outstanding
{
    public class OutstandingHandler : IRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            return null;
        }
    }
}