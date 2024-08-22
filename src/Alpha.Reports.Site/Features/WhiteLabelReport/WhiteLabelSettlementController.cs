namespace Alpha.Reports.Site.Features.WhiteLabelReport
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.WhiteLabelReport.Requests;
    using Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels;
    using AlphaPlus.User.Models;
    using MediatR;

    public class WhiteLabelSettlementController : ReportBaseController
    {
        private readonly string viewPath = "~/Features/WhiteLabelReport/Views/{0}.cshtml";

        public WhiteLabelSettlementController(IMediator mediator) : base(mediator)
        {
        }

        public Task<ActionResult> Index(SettlementQuery message)
            => ViewAsync<SettlementViewModel>(string.Format(viewPath, "Settlement"), message);

        public Task<ActionResult> GetData(SettlementGetDataQuery message)
            => ViewAsync(string.Format(viewPath, "SettlementData"), message);

        public Task<FileContentResult> Excel(SettlementExcelQuery message) => ExcelViewAsync(message);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!SiteSettings.IsOfferedWhiteLabel(UserSession.Current.Site))
            {
                UnAuthentication(filterContext);
            }
        }
    }
}