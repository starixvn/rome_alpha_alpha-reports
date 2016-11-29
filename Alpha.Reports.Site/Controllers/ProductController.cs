namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Services;
    using Web;

    public class ProductController : BaseController
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
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