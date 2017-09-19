namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLoss;

    public class EnteredResultController : ReportBaseController
    {
        public EnteredResultController(IMediator mediator) : base(mediator)
        {
        }

        [Route("enteredresult/master")]
        [Route("enteredresult/agent")]
        [Route("enteredresult/member")]
        public ActionResult Index(EnteredSportResultQuery message) => View(message);

        [HttpGet]
        public ActionResult Excel(EnteredSportResultExcelQuery message) => ExcelView(message);
    }
}