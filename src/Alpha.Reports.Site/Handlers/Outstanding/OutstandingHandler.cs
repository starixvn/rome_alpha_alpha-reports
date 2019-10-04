namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Requests.Outstanding;
    using Services;
    using Services.Outstanding;
    using ViewModels.Outstanding;
    using System.Linq;

    public class OutstandingHandler : BaseHandler,
        IRequestHandler<OutstandingQuery, OutstandingViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private readonly OutstandingService outstandingService;
        private readonly ProductService productService;

        public OutstandingHandler(
            OutstandingService outstandingServiceImpl,
            ProductService productServiceImpl,
            IUser viewer)
        {
            outstandingService = outstandingServiceImpl;
            productService = productServiceImpl;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.CustId, message.UserSelectedProductIds.ToListInt(), DateTime.Now);
            
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

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            return Enumerable.Empty<string>();
        }
    }
}