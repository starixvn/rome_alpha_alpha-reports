namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.PokerGamesWinLossDetail;
    using ViewModels.PokerGamesWinLossDetail;

    public class PokerGamesWinLossDetailController : ReportBaseController
    {
        public PokerGamesWinLossDetailController(IMediator mediator) : base(mediator)
        {
        }

        [Route("pokergameswinlossdetail/master")]
        [Route("pokergameswinlossdetail/agent")]
        [Route("pokergameswinlossdetail/member")]
        public ActionResult Index(PokerGamesWinLossDetailQuery message)
            => View<PokerGamesWinLossDetailViewModel>(message);

        [HttpGet]
        public ActionResult Excel(PokerGamesWinLossDetailExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(PokerGamesReportLegendQuery message) => View(message);
    }
}