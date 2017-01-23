namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossByBettype;

    public class WinLossByBettypeController : ReportBaseController
    {
        public WinLossByBettypeController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(WinLossByBettypeQuery message)
            => View(message);
    }
}