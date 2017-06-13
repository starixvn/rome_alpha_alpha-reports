namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests;

    public class LeagueController : ReportBaseController
    {
        public LeagueController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult LeaguesByCustomer(LeaguesByCustomerQuery message) => View(message);
    }
}