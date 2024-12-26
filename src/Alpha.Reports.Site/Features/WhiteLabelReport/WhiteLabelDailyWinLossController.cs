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

    public class WhiteLabelDailyWinLossController : ReportBaseController
    {
        private readonly string viewPath = "~/Features/WhiteLabelReport/Views/{0}.cshtml";

        public WhiteLabelDailyWinLossController(IMediator mediator) : base(mediator)
        {
        }

        public Task<ActionResult> Index(DailyWinLossQuery message)
            => ViewAsync<DailyWinLossViewModel>(string.Format(viewPath, "DailyWinLoss"), message);

        public Task<ActionResult> GetData(DailyWinLossGetDataQuery message)
            => ViewAsync(string.Format(viewPath, "DailyWinLossData"), message);

        public Task<FileContentResult> Excel(DailyWinLossExcelQuery message) => ExcelViewAsync(message);

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