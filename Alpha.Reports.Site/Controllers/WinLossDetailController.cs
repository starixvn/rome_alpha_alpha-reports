namespace Alpha.Reports.Site.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
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

        public ActionResult Member()
        {
            var products = new List<Product>();
            //{
            //    new Product(1, "SB", string.Empty),
            //    new Product(2, "SB", string.Empty),
            //    new Product(3, "SB", string.Empty)
            //};

            var message = new MemberWinLossDetailQuery(products, userSession);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}