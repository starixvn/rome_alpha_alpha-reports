namespace Alpha.Reports.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using Configurations;
    using Helpers;
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using Requests.WinLossDetail;
    using Sunplus.Agent.Model.Entities;

    public class WinLossDetailController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public WinLossDetailController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        public ActionResult Master(WinLossDetailQuery<MasterWinLossDetail> message)
        {
            UpdateQuery(message);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        public ActionResult Agent(WinLossDetailQuery<AgentWinLossDetail> message)
        {
            UpdateQuery(message);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        public ActionResult Member(WinLossDetailQuery<MemberWinLossDetail> message)
        {
            UpdateQuery(message);
            var viewModel = mediator.Send(message);

            return View(viewModel);
        }

        private void UpdateQuery<T>(WinLossDetailQuery<T> message) where T: MemberWinLossDetail
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var type = typeof(T);

            if (type == typeof(MemberWinLossDetail))
            {
                message.UserLevel = UserLevel.Agent;
            }
            else if (type == typeof(AgentWinLossDetail))
            {
                message.UserLevel = UserLevel.Master;
            } 
            else
            {
                message.UserLevel = UserLevel.Member;
            }

            message.Language = userSession.Language;

            if (message.CustId <= 0)
            {
                message.CustId = userSession.CustId;
            }            
        }        
    }
}