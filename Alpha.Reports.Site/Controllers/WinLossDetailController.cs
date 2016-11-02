namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.WinlossDetail;
    using Requests.WinLossDetail;

    public class WinLossDetailController : BaseController
    {
        private readonly IMediator mediator;        

        public WinLossDetailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ActionResult Master(WinLossDetailQuery<MasterWinLossDetail> message)
        {
            return GetActionResult(message);
        }

        public ActionResult Agent(WinLossDetailQuery<AgentWinLossDetail> message)
        {
            return GetActionResult(message);
        }

        public ActionResult Member(WinLossDetailQuery<MemberWinLossDetail> message)
        {
            return GetActionResult(message);
        }
        
        private ActionResult GetActionResult<T>(WinLossDetailQuery<T> message) where T : MemberWinLossDetail
        {
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }       
    }
}