using Alpha.Reports.Enumerations;
using Alpha.Reports.Models;
using Alpha.Reports.Services.Outstanding;
using Alpha.Reports.Site.Requests.Outstanding;
using Alpha.Reports.Site.ViewModels.Outstanding;
using MediatR;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Reports.Site.Handlers.Outstanding
{
    public class OutstandingHandler : IRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        private readonly OutstandingService _outstandingServiceImpl;
        private readonly IUserSession _userSession;

        public OutstandingHandler(OutstandingService outstandingServiceImpl, IUserSession userSession)
        {
            _outstandingServiceImpl = outstandingServiceImpl;
            _userSession = userSession;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var userLevel = Enumeration.FromValue<UserLevel>(_userSession.RoleId);
            var custLevel = Enumeration.FromValue<UserLevel>(3);

            var outstandings = _outstandingServiceImpl.GetMemberOutstanding(userLevel, custLevel, message.CustId);


            return new OutstandingViewModel();
        }
    }
}