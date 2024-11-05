namespace Alpha.Reports.Site.Handlers.Outstanding
{
    using Alpha.Web.Helpers;
    using Extensions;
    using MediatR;
    using Params;
    using Reports.Models;
    using Requests.Outstanding;
    using Services;
    using Services.Outstanding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Outstanding;

    public class OutstandingHandler : BaseHandler,
        IRequestHandler<OutstandingQuery, OutstandingViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>
    {
        private readonly OutstandingService outstandingService;
        private readonly ProductService productService;
        private readonly BrowserHelper browserHelper;

        // Always hide Saba Promotion in Product List
        private readonly int SabaPromotionProductId = 37;

        public OutstandingHandler(
            OutstandingService outstandingServiceImpl,
            ProductService productServiceImpl,
            BrowserHelper browserHelper,
            IUser viewer)
        {
            outstandingService = outstandingServiceImpl;
            productService = productServiceImpl;
            this.browserHelper = browserHelper;
        }

        public OutstandingViewModel Handle(OutstandingQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIds(message.Site, message.CurrencyId, message.UserSelectedProductIds.ToListInt(), DateTime.Now).Where(id => id != SabaPromotionProductId);
            
            var param = new OutstandingParam(
                message.ViewerLevel,
                message.CustLevel,
                message.CustId,
                validUserSelectedProductIds.JoinWithComma(),
                message.AutoFollowBetIsDummyAgent,
                message.LevelType);

            var outstandings = outstandingService.GetMemberOutstanding(param);
            var products = productService.GetAllOfferedProducts(message.Site, message.CurrencyId, DateTime.Now).Where(p => p.Id != SabaPromotionProductId);
            var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

            var viewModel = new OutstandingViewModel(
                message,
                outstandings,
                validUserSelectedProductIds,
                BuildProductSelectListItems(validUserSelectedProductIds, products),
                BuildBreadcrumbLevel(message),
                isOnlyMobile);

            return viewModel;
        }

        public IEnumerable<string> Handle(ReportLegendQuery message)
        {
            return Enumerable.Empty<string>();
        }
    }
}