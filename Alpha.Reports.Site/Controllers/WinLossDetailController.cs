namespace Alpha.Reports.Site.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using MediatR;
    using Models;
    using Requests.WinLossDetail;
    using Sunplus.Agent.Model.Entities;

    public class WinLossDetailController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public WinLossDetailController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        public ActionResult Master()
        {
            return View();
        }

        public ActionResult Agent()
        {
            return View();
        }

        public ActionResult Member(MemberWinLossDetailQuery message)
        {
            message.Language = userSession.Language;
            message.CustId = userSession.CustId;
            message.ReportPath = AppPathHelper.GetRelativePath(SiteSettings.ReportsAppPath, HttpContext.Request.Url.AbsolutePath);
            message.OldMainAppPath = AppPathHelper.GetAppPath(SiteSettings.OldMainAppPath, "~");
            message.Products = SiteSettings.Products;
            message.ReportRanges = SiteSettings.ReportRanges;

            message.SelectedProducts = "1,2";

            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}