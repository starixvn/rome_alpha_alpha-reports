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
        private readonly IUser user;

        public OutstandingHandler(OutstandingService outstandingService, IUser user)
        {
            this.outstandingService = outstandingService;
            this.user = user;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var reportLevel = Enumeration.FromValue<UserLevel>(message.CustLevelId);

            IEnumerable<OutstandingData> outstandings;
            IEnumerable<CustInfo> drillDownInfo;
            int nextCustLevel;
            if (message.CustLevelId == 0)
            {
                outstandings = outstandingService.GetMemberOutstanding(reportLevel, reportLevel, message.CustId);
                nextCustLevel = reportLevel.Value - 1;
                drillDownInfo = BuildDrillDownInfo(message);
            }
            else
            {
                outstandings = outstandingService.GetMemberOutstanding(reportLevel, Enumeration.FromValue<UserLevel>(message.CustLevelId), message.CustId);
                nextCustLevel = message.CustLevelId - 1;
                drillDownInfo = BuildDrillDownInfo(message);
            }

            var viewModel = new OutstandingViewModel(
                reportLevel,
                Enumeration.FromValue<UserLevel>((byte)nextCustLevel),
                outstandings,
                ToJson(drillDownInfo),
                BuildBreadcrumbLevels(
                    drillDownInfo,
                    message,
                    "Outstanding",
                    "Index",
                    (custInfo, query, drillDownInfoByLevelJson) =>
                    {
                        var routeValues = new OutstandingQuery(user);
                        routeValues.CustLevelId = custInfo.CustLevelId;
                        routeValues.CustId = custInfo.CustId;
                        routeValues.CustName = custInfo.CustName;
                        routeValues.DrillDownInfoJson = drillDownInfoByLevelJson;

                        return routeValues;
                    }));

            return viewModel;
        }
    }
}