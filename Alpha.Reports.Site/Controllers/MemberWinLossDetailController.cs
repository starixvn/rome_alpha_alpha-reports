namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using Reports.Helpers;
    using ViewModels.MemberWinLossDetail;

    public class MemberWinLossDetailController : BaseController
    {
        public ActionResult Index()
        {
            return View(new IndexViewModel
            {
                MinDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now.AddMonths(-2)),
                CurrentDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now)
            });
        }
    }
}