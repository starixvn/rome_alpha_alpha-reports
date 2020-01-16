namespace Alpha.Reports.Site.Controllers
{
    using Alpha.Reports.Site.Requests;
    using MediatR;
    using System.Web.Mvc;

    public class RiskAnalysisIPMatchingController : ReportBaseController
    {
        public RiskAnalysisIPMatchingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public ActionResult Index(RiskAnalysisIPMatchingQuery message)
            => View(message);

        //[HttpGet]
        //public ActionResult Excel(WinLossDetailExcelQuery message)
        //    => ExcelView(message);
    }
}