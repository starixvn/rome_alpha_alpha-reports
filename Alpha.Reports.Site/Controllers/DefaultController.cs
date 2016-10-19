namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;

    public class DefaultController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MasterWinLossDetail()
        {
            return View();
        }
    }
}