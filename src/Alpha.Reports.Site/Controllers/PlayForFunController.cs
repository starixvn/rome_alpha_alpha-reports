namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.PlayForFun;

    public class PlayForFunController : ReportBaseController
    {
        public PlayForFunController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult MasterOrAgent(MasterOrAgentPlayForFunQuery message)
            => View(message);

        public ActionResult Member(MemberPlayForFunQuery message)
            => View(message);

        [HttpGet]
        public ActionResult MasterOrAgentExcel(MasterOrAgentPlayForFunExcelQuery message)
            => ExcelView(message);

        [HttpGet]
        public ActionResult MemberExcel(MemberPlayForFunExcelQuery message)
            => ExcelView(message);
    }
}