namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

    public class OutstandingHandler : BaseHandler,
        IRequestHandler<OutstandingQuery, OutstandingViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private readonly OutstandingService outstandingService;
        private readonly ProductService productService;
        private readonly IUser viewer;

        public OutstandingHandler(
            OutstandingService outstandingServiceImpl,
            ProductService productServiceImpl,
            IUser viewer)
        {
            outstandingService = outstandingServiceImpl;
            productService = productServiceImpl;
            this.viewer = viewer;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var products = productService.GetProductsForCustomer(message.CustId, DateTime.Now).ToList();

            if (!viewer.GetHideProduct(ProductIdConstants.PokerGames)
                && !products.Any(product => product.Id == ProductIdConstants.PokerGames))
            {
                products.Add(productService.GetPokerGames());
            }

            var validUserSelectedProductIds =
                (string.IsNullOrWhiteSpace(message.UserSelectedProductIds)
                    ? products
                    : products.Where(product => message.UserSelectedProductIds.ToListInt().Contains(product.Id)))
                .Select(product => product.Id);

            var param = new OutstandingParam(
                message.ViewerLevel,
                message.CustLevel,
                message.CustId,
                validUserSelectedProductIds.JoinWithComma());

            var outstandings = outstandingService.GetMemberOutstanding(param);

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
            var result = new List<string>();

            if (productService.IsShowProduct(viewer.Id, ProductIdConstants.PokerGames)
                || !viewer.GetHideProduct(ProductIdConstants.PokerGames))
            {
                result.Add(Resources.PokerGames_OutstandingNote);
            }

            return result;
        }
    }
}