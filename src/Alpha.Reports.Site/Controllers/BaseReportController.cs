namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseReportController : BaseController
    {
    }
}