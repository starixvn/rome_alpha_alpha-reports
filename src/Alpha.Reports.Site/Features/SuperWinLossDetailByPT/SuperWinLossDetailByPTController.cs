namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT
{
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.SuperWinLossDetailByPT.Requests;
    using MediatR;

    public class SuperWinLossDetailByPTController : ReportBaseController
    {
        private const string IndexViewPath = "~/Features/SuperWinLossDetailByPT/Views/Index.cshtml";
        private const string ReportLegendViewPath = "~/Features/SuperWinLossDetailByPT/Views/ReportLegend.cshtml";

        public SuperWinLossDetailByPTController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(SuperWinLossDetailByPTQuery message)
            => View(IndexViewPath, message);

        public ActionResult Excel(SuperWinLossDetailByPTExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View(ReportLegendViewPath, message);
    }
}