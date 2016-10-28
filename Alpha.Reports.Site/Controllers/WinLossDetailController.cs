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
            message.CustId = string.IsNullOrWhiteSpace(message.CustId) ? userSession.CustId.ToString() : message.CustId;
            message.ReportPath = AppPathHelper.GetRelativePath(AppSettings.ReportsAppPath, HttpContext.Request.Url.AbsolutePath);

            message.SelectedProducts = "1,2,3";
            message.Products = new List<Product>()
            {
                new Product(1, "SB", string.Empty),
                new Product(2, "SB", string.Empty),
                new Product(3, "SB", string.Empty)
            };


            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}