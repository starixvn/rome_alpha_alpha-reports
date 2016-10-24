namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Configurations;
    using MediatR;
    using Requests.MemberWinLossDetail;

    public class MemberWinLossDetailController : BaseController
    {
        private readonly IMediator mediator;

        public MemberWinLossDetailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Index()
        {
            var message = new IndexQuery(AppSettings.Products, FunctionRegistry.ProductPermissions);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }
    }
}