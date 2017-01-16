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
        private readonly OutstandingService outstandingService;

        public OutstandingHandler(OutstandingService outstandingService)
        {
            this.outstandingService = outstandingService;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var reportLevel = Enumeration.FromValue<UserLevel>(message.CustLevelId);

            IEnumerable<OutstandingData> outstandings;
            IEnumerable<CustInfo> drillDownInfo = BuildBreadcrumbLevels(message);
            int nextCustLevel;
            if (message.CustLevelId == 0)
            {
                outstandings = outstandingService.GetMemberOutstanding(reportLevel, reportLevel, message.CustId);
                nextCustLevel = reportLevel.Value - 1;
            }
            else
            {
                outstandings = outstandingService.GetMemberOutstanding(reportLevel, Enumeration.FromValue<UserLevel>(message.CustLevelId), message.CustId);
                nextCustLevel = message.CustLevelId - 1;
            }

            var viewModel = new OutstandingViewModel(
               Enumeration.FromValue<UserLevel>(message.LoginUserLevelId),
               Enumeration.FromValue<UserLevel>((byte)nextCustLevel),
               outstandings,
               ToJson(drillDownInfo),
               drillDownInfo);

            return viewModel;
        }
    }
}