namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Configurations;
    using Sunplus.Agent.Model.Entities;

    public class DefaultController : BaseController
    {
        public DefaultController(IUserSession userSession) : base(userSession)
        {
        }

        public ActionResult Index()
        {
            var products = AppSettings.Products;

            return View();
        }
    }
}