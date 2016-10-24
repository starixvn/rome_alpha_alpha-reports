namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Configurations;
    using MediatR;
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
            var visibleProducts = productService.GetVisibleProducts(new GetVisibleProductsParam(AppSettings.Products, FunctionRegistry.ProductPermissions));

            var message = new IndexQuery(visibleProducts, userSession);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}