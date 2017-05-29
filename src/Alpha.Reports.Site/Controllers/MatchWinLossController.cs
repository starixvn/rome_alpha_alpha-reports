namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.MatchWinLoss;

    public class MatchWinLossController : ReportBaseController
    {
        public MatchWinLossController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(MatchWinLossQuery message) => View(message);

        [HttpGet]
        public ActionResult Excel(MatchWinLossExcelQuery message) => ExcelView(message);
    }
}