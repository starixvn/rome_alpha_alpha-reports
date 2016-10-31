namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Configurations;
    using Services;
    using Services.Impl;

    public class ProductController : BaseController
    {
        private readonly ProductService productService;

        public ProductController(ProductServiceImpl productService)
        {
            this.productService = productService;
        }

        public JsonResult GetProductsForCustomer(int custId)
        {
            var products = productService.GetProductsForCustomer(custId);

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}