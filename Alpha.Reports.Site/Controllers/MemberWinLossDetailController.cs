namespace Alpha.Reports.Site.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using MediatR;
    using Models;
    using Params;
    using Requests.MemberWinLossDetail;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class MemberWinLossDetailController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;
        private ProductService productService;

        public MemberWinLossDetailController(
            IUserSession userSession,
            IMediator mediator,
            ProductService productService)
        {
            this.userSession = userSession;
            this.mediator = mediator;
            this.productService = productService;
        }

        public ActionResult Index()
        {
            // var products = productService.GetProductsByCustomer(new GetProductByCustomerParam(userSession.CustId));
            var products = new List<Product>
            {
                new Product(1, "SB", string.Empty),
                new Product(2, "SB", string.Empty),
                new Product(3, "SB", string.Empty)
            };

            var message = new IndexQuery(products, userSession);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}