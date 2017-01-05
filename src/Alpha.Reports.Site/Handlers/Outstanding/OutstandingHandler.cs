using System.Collections.Generic;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using Enumerations;
    using Reports.Models;
    using Services.Outstanding;
    using Requests.Outstanding;
    using ViewModels.Outstanding;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class OutstandingHandler : IRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        private readonly OutstandingService outstandingServiceImpl;
        private readonly IUserSession userSession;

        public OutstandingHandler(OutstandingService outstandingServiceImpl, IUserSession userSession)
        {
            this.outstandingServiceImpl = outstandingServiceImpl;
            this.userSession = userSession;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var userLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);

            IEnumerable<OutstandingModel> outstandings;
            int nextCustLevel;
            if (message.CustLevelId == 0)
            {
                outstandings = outstandingServiceImpl.GetMemberOutstanding(userLevel, userLevel, userSession.CustId);
                nextCustLevel = userLevel.Value - 1;
            }
            else
            {
                outstandings = outstandingServiceImpl.GetMemberOutstanding(userLevel, Enumeration.FromValue<UserLevel>(message.CustLevelId), message.CustId);
                nextCustLevel = message.CustLevelId - 1;
            }

            var viewModel = new OutstandingViewModel
            {
                Outstandings = outstandings,
                CustLevel = Enumeration.FromValue<UserLevel>((byte)nextCustLevel)
            };

            return viewModel;
        }
    }
}