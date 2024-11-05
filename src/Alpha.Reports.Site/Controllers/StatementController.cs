namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.SecurityCode.Filters;
    using MediatR;

    [SecurityCodeFilter(isStatement: true)]
    public class StatementController : ReportBaseController
    {
        public StatementController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult SMAStatement(SMAStatementQuery message) => View(message);

        public ActionResult SMAReportLegend() => View();

        public ActionResult MemberReportLegend(bool? isDirectMember) => View(isDirectMember);

        public ActionResult StatementDetails(StatementDetailsQuery message) => View("_StatementDetails", message);

        public ActionResult MemberStatement(MemberStatementQuery message) => View(message);

        [HttpGet]
        public ActionResult SMAStatementExcel(SMAStatementExcelQuery message)
            => ExcelView(message);

        [HttpGet]
        public ActionResult StatementDetailExcel(StatementDetailExcelQuery message)
            => ExcelView(message);

        [HttpGet]
        public ActionResult MemberStatementExcel(MemberStatementExcelQuery message)
            => ExcelView(message);
    }
}