namespace Alpha.Reports.Site.Controllers
{
    using MediatR;
    using Requests.WinLossAnalysisChart;
    using System.Web.Mvc;
    using ViewModels.WinLossAnalysisChart;

    public class WinLossAnalysisChartController : ReportBaseController
    {
        public WinLossAnalysisChartController(IMediator mediator) : base(mediator)
        {
        }

        [Route("winlossanalysischart/master")]
        [Route("winlossanalysischart/agent")]
        [Route("winlossanalysischart/member")]
        public ActionResult Index(WinLossAnalysisChartQuery message) => View(message);
    }
}