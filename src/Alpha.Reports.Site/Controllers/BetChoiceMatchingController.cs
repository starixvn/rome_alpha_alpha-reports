namespace Alpha.Reports.Site.Controllers
{
    using Alpha.Reports.Site.Requests;
    using Alpha.Reports.Site.ViewModels.BetChoiceMatching;
    using MediatR;
    using System.Web.Mvc;

    public class BetChoiceMatchingController : ReportBaseController
    {
        public BetChoiceMatchingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index()
            => View(new BetChoiceMatchingViewModel());

        [HttpPost]
        public ActionResult Query(BetChoiceMatchingQuery message)
            => Json(message);
    }
}