namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using App_Start;
    using Configurations;
    using MediatR;
    using Requests;

    public class ProductController : BaseController
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public JsonResult VisibleProducts()
        {
            var message = new VisibleProductsQuery(AppSettings.Products, FunctionRegistry.ProductPermissions);
            var viewModel = mediator.Send(message);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}