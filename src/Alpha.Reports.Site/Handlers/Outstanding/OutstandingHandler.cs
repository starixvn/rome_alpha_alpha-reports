namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using System.Collections.Generic;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Requests.Outstanding;
    using Services.Outstanding;
    using ViewModels.Outstanding;

    public class OutstandingHandler : BaseHandler, IRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        private readonly OutstandingService outstandingServiceImpl;

        public OutstandingHandler(OutstandingService outstandingServiceImpl)
        {
            this.outstandingServiceImpl = outstandingServiceImpl;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var viwerLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            IEnumerable<OutstandingData> outstandings;
            int nextCustLevel;
            if (message.CustLevelId == 0)
            {
                outstandings = outstandingServiceImpl.GetMemberOutstanding(viwerLevel, viwerLevel, message.CustId);
                nextCustLevel = viwerLevel.Value - 1;
            }
            else
            {
                outstandings = outstandingServiceImpl.GetMemberOutstanding(viwerLevel, Enumeration.FromValue<UserLevel>(message.CustLevelId), message.CustId);
                nextCustLevel = message.CustLevelId - 1;
            }

            var viewModel = new OutstandingViewModel(viwerLevel, Enumeration.FromValue<UserLevel>((byte)nextCustLevel), outstandings);

            return viewModel;
        }
    }
}