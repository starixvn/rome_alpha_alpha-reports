namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using MediatR;
    using Requests.Outstanding;
    using Services.Outstanding;
    using ViewModels.Components;
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
            var outstandingDataSource = outstandingService.GetMemberOutstanding(message.ViewerLevel, message.CustLevel, message.CustId);
            var nextCustLevel = message.CustLevel.GetDirectDownLineLevel();

            var drillDownInfo = BuildBreadcrumbLevel(message);
            var breadcrumbLevelsViewModel = new BreadcrumbLevelViewModel(drillDownInfo);

            var viewModel = new OutstandingViewModel(
                message.ViewerLevel,
                nextCustLevel,
                outstandingDataSource,
                breadcrumbLevelsViewModel);

            return viewModel;
        }
    }
}