namespace Alpha.Reports.Site.Controllers
{
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using MediatR;
    using System.Web.Mvc;

    public class BetChoiceMatchingController : ReportBaseController
    {
        public BetChoiceMatchingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index(BetChoiceMatchingQuery message) => View(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Query(BetChoiceMatchingJsonQuery message) => Json(message);

        [HttpGet]
        public ActionResult Excel(BetChoiceMatchingExcelQuery message) => ExcelView(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Uplines(BetChoiceUplinesJsonQuery message) => Json(message);
    }
}