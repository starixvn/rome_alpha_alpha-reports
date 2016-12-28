namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLoss;
    using ViewModels;

    public class WinLossController : ReportControllerBase
    {
        public WinLossController(IMediator mediator) 
            : base(mediator)
        {
        }

        [Route("enteredresult/super")]
        [Route("enteredresult/master")]
        [Route("enteredresult/agent")]
        public ActionResult EnteredResult(EnteredMatchResultQuery message)
            => View(mediator.Send(message));

        [HttpGet]
        public ActionResult EnteredResultExcel(EnteredMatchResultExcelQuery message)
            => ExcelFile(mediator.Send<ExcelFileViewModel>(message));

        public ActionResult WinLoss(WinLossQuery message)
            => View(mediator.Send(message));

        [HttpGet]
        public ActionResult WinLossExcel(WinLossExcelQuery message)
            => ExcelFile(mediator.Send<ExcelFileViewModel>(message));      
    }
}