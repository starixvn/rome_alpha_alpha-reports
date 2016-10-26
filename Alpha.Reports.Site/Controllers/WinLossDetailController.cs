namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class WinLossDetailController : BaseController
    {
        // GET: WinLossDetail
        public ActionResult Master()
        {
            return View();
        }

        public ActionResult Agent()
        {
            return View();
        }

        public ActionResult Member()
        {
            return View();
        }
    }
}