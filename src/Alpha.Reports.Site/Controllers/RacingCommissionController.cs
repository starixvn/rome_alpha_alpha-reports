using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Alpha.Reports.Site.Requests.RacingCommission;
using MediatR;

namespace Alpha.Reports.Site.Controllers
{
    public class RacingCommissionController : ReportBaseController
    {
        public RacingCommissionController(IMediator mediator) : base(mediator)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [Route("racingcommission/master")]
        [Route("racingcommission/agent")]
        [Route("racingcommission/member")]
        public ActionResult Index(RacingCommissionQuery message) => View(message);

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Reviewed")]
        [HttpGet]
        public ActionResult Excel(RacingCommissionExcelQuery message) => ExcelView(message);
    }
}