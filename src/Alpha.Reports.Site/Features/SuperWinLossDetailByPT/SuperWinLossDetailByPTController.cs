namespace Alpha.Reports.Site.Controllers
{
    using MediatR;
    using Requests.SuperWinLossDetailByPT;
    using System.Web.Mvc;

    public class SuperWinLossDetailByPTController : ReportBaseController
    {
        private const string indexViewPath = "~/Features/SuperWinLossDetailByPT/Views/Index.cshtml";
        private const string reportLegendViewPath = "~/Features/SuperWinLossDetailByPT/Views/ReportLegend.cshtml";

        public SuperWinLossDetailByPTController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(SuperWinLossDetailByPTQuery message)
            => View(indexViewPath, message);

        public ActionResult Excel(SuperWinLossDetailByPTExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View(reportLegendViewPath, message);
    }
}