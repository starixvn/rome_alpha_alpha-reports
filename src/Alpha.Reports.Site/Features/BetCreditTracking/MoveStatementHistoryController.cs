namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
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

        public async Task<ActionResult> Index(MoveStatementHistoryQuery message)
            => await ViewAsync(string.Format(basePath, nameof(Index)), message);

        public async Task<ActionResult> Excel(MoveStatementHistoryExcelQuery message)
            => await ExcelViewAsync(message);
    }
}