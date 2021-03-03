namespace Alpha.Reports.Site.Features.BonusDetail
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.BonusDetail.Requests;
    using Alpha.Reports.Site.Features.BonusDetail.ViewModels;
    using Alpha.Web.Filters;
    using MediatR;

    public class BonusDetailController : ReportBaseController
    {
        private readonly string basePath = "~/Features/BonusDetail/Views/{0}.cshtml";

        public BonusDetailController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BonusDetailQuery message)
            => await ViewAsync(string.Format(basePath, nameof(Index)), message);

        public ActionResult ConfirmPayment(ConfirmPaymentQuery message)
            => message.IsValid()
            ? View(string.Format(basePath, nameof(ConfirmPayment)), new ConfirmPaymentViewModel(message))
            : View(InvalidReportViewPath);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> ConfirmPayment(ConfirmPaymentCommand message)
            => await JsonAsync(message);
    }
}