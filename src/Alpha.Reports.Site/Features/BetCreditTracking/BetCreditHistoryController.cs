namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using MediatR;

    public class BetCreditHistoryController : ReportBaseController
    {
        private readonly string basePath = "~/Features/BetCreditTracking/Views/BetCreditHistory/{0}.cshtml";

        public BetCreditHistoryController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BetCreditHistoryQuery message)
            => await ViewAsync(string.Format(basePath, nameof(Index)), message);

        public async Task<ActionResult> Excel(BetCreditHistoryExcelQuery message)
            => await ExcelViewAsync(message);
    }
}