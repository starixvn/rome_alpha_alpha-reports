namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossAnalysis;

    public class WinLossAnalysisController : ReportBaseController
    {
        public WinLossAnalysisController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossanalysis/master")]
        [Route("winlossanalysis/agent")]
        [Route("winlossanalysis/member")]
        public ActionResult Index(WinLossAnalysisQuery message)
            => View(message);

        [HttpGet]
        public ActionResult Excel(WinLossAnalysisExcelQuery message)
            => ExcelView(message);
    }
}