namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.SecurityCode.Filters;
    using MediatR;

    [SecurityCodeFilter(isBetLists: true)]
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