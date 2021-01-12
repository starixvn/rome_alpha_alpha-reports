namespace Alpha.Reports.Site.Features.BonusDetail
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.BonusDetail.Requests;
    using Alpha.Web.Filters;
    using MediatR;

    public class BonusDetailController : ReportBaseController
    {
        public BonusDetailController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BonusDetailQuery message)
            => await ViewAsync("~/Features/BonusDetail/Views/Index.cshtml", message);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> ConfirmPayment(ConfirmPaymentCommand message)
            => await JsonAsync(message);
    }
}