namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using System;
    using Extensions;
    using MediatR;
    using Params;
    using Requests.Outstanding;
    using Services;
    using Services.Outstanding;
    using ViewModels.Outstanding;

    public class OutstandingHandler : BaseHandler, IRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        private readonly OutstandingService outstandingService;
        private readonly ProductService productService;

        public OutstandingHandler(OutstandingService outstandingServiceImpl, ProductService productServiceImpl)
        {
            outstandingService = outstandingServiceImpl;
            productService = productServiceImpl;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.CustId, message.UserSelectedProductIds.ToListInt());

            var param = new OutstandingParam(
                message.ViewerLevel,
                message.CustLevel,
                message.CustId,
                validUserSelectedProductIds.JoinWithComma());

            var outstandings = outstandingService.GetMemberOutstanding(param);

            var products = productService.GetProductsForCustomer(message.CustId, DateTime.Now);

            var viewModel = new OutstandingViewModel(
                message,
                outstandings,
                validUserSelectedProductIds,
                BuildProductSelectListItems(validUserSelectedProductIds, products),
                BuildBreadcrumbLevel(message));

            return viewModel;
        }
    }
}