namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests;

    public class ProductController : BaseController
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public JsonResult GetAllProducts(GetAllProductsQuery message)
        {
            var products = mediator.Send(message);

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}