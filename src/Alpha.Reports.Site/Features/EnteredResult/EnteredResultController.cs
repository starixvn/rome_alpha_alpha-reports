namespace Alpha.Reports.Site.Controllers
{
    using MediatR;
    using Requests.WinLoss;
    using System.Web.Mvc;

    public class EnteredResultController : ReportBaseController
    {
        private readonly string enteredResultPath = "~/Features/EnteredResult/Views/Index.cshtml";
        private readonly string enteredResultNewPath = "~/Features/EnteredResult/Views/IndexNew.cshtml";

        public EnteredResultController(IMediator mediator) : base(mediator)
        {
        }

        [Route("enteredresult/master")]
        [Route("enteredresult/agent")]
        [Route("enteredresult/member")]
        public ActionResult Index(EnteredSportResultQuery message)
            => View(enteredResultPath, message);

        [Route("enteredresult/masternew")]
        [Route("enteredresult/agentnew")]
        [Route("enteredresult/membernew")]
        public ActionResult IndexNew(EnteredSportResultQuery message)
            => View(enteredResultNewPath, message);

        [HttpGet]
        public ActionResult Excel(EnteredSportResultExcelQuery message)
            => ExcelView(message);
    }
}