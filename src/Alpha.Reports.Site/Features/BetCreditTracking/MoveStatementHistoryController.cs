namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using MediatR;

    public class MoveStatementHistoryController : ReportBaseController
    {
        private readonly string basePath = "~/Features/BetCreditTracking/Views/MoveStatementHistory/{0}.cshtml";

        public MoveStatementHistoryController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(MoveStatementHistoryQuery message)
            => View(string.Format(basePath, nameof(Index)), message);

        public ActionResult Excel(MoveStatementHistoryExcelQuery message)
            => ExcelView(message);
    }
}