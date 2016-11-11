namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossDetails;

    public class WinLossDetailController : BaseController
    {
        private readonly IMediator mediator;

        public WinLossDetailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index(WinLossDetailQuery message)
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}