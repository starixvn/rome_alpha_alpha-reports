namespace Alpha.Reports.Site.Controllers
{
    using MediatR;
    using Requests.SuperWinLossDetailByPT;
    using System.Web.Mvc;

    public class SuperWinLossDetailByPTController : ReportBaseController
    {
        public SuperWinLossDetailByPTController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(SuperWinLossDetailByPTQuery message)
            => View(message);

        public ActionResult Excel(SuperWinLossDetailByPTExcelQuery message)
            => ExcelView(message);
    }
}