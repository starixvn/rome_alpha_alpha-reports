namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Reports.Models;
    using Requests.ProgressiveContribution;

    public class ProgressiveContributionController : ReportBaseController
    {
        private readonly IUser viewer;

        public ProgressiveContributionController(IMediator mediator, IUser viewer)
            : base(mediator)
        {
            this.viewer = viewer;
        }

        public ActionResult Index(ProgressiveContributionQuery message)
            => View(message);

        public ActionResult Excel(ProgressiveContributionExcelQuery message)
            => ExcelView(message);

        public ActionResult ReportLegend(ReportLegendQuery message)
            => View(message);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!viewer.IsEnabledJackpot)
            {
                UnAuthentication(filterContext);
            }
        }
    }
}
