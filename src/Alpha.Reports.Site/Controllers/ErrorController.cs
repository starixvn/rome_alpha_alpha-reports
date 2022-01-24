namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Filters;

    [AllowAnonymous, AllowProtectedMode, SessionState(SessionStateBehavior.ReadOnly)]
    public class ErrorController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult Blocked() => View();
    }
}