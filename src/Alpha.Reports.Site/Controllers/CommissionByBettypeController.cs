namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Requests.CommissionByBettype;

    public class CommissionByBettypeController : Controller
    {
        [Route("commissionbybettype/master")]
        [Route("commissionbybettype/agent")]
        [Route("commissionbybettype/member")]
        public ActionResult Index(CommissionByBettypeQuery message) => View(message);
    }
}