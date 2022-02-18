namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using MediatR;

    public class StatementMovementController : ReportBaseController
    {
        private readonly string basePath = "~/Features/BetCreditTracking/Views/StatementMovement/{0}.cshtml";

        public StatementMovementController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(StatementMovementQuery message)
            => await ViewAsync(string.Format(basePath, nameof(Index)), message);

        public async Task<ActionResult> Excel(StatementMovementExcelQuery message)
            => await ExcelViewAsync(message);
    }
}