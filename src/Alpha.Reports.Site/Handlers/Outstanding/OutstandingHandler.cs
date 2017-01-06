namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using System.Collections.Generic;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Requests.Outstanding;
    using Services.Outstanding;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Outstanding;

    public class OutstandingHandler : BaseHandler, IRequestHandler<OutstandingQuery, OutstandingViewModel>
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