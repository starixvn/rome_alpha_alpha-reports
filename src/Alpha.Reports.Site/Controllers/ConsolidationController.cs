namespace Alpha.Reports.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Requests.Consolidation;
    using ViewModels.Consolidation;

    public class ConsolidationController : ReportBaseController
    {
        private readonly IUser viewer;

        public ConsolidationController(IMediator mediator, IUser viewer)
            : base(mediator)
        {
            this.viewer = viewer;
        }

        public ActionResult ReportLegend(ConsolidationLegendQuery message) => View(message);

        public ActionResult ReportDetailLegend(ConsolidationDetailLegendQuery message) => View(message);

        public ActionResult ConsolidationByUsername(ConsolidationByUsernameQuery message)
            => View(message);

        public ActionResult ConsolidationByDate(ConsolidationByDateQuery message)
            => View<ConsolidationReportByDateViewModel>(message);

        public ActionResult ConsolidationByProduct(ConsolidationByProductQuery message)
            => View<ConsolidationReportByProductViewModel>(message);

        public ActionResult ConsolidationDetail(ConsolidationDetailQuery message)
            => View(message);

        [HttpGet]
        public ActionResult ConsolidationByUsernameExcel(ConsolidationByUsernameExcelQuery message)
            => ExcelView(message);

        [HttpGet]
        public ActionResult ConsolidationByDateExcel(ConsolidationByDateExcelQuery message)
            => ExcelView(message);

        [HttpGet]
        public ActionResult ConsolidationByProductExcel(ConsolidationByProductExcelQuery message)
            => ExcelView(message);

        [HttpGet]
        public ActionResult ConsolidationDetailExcel(ConsolidationDetailExcelQuery message)
        {
            var reportData = mediator.Send<PagedReportData<WinLossDetailModel>>(message);

            var query = new ConsolidateWinLossDetailExcelQuery(viewer)
            {
                UserSelectedProductIds = message.UserSelectedProductIds,
                UserSelectedResourceIds = message.UserSelectedResourceIds,
                SortPropertyName = message.SortPropertyName,
                IsDescendingSort = message.IsDescendingSort,
                IsStackMode = message.IsStackMode,
                IsFilterVisible = message.IsFilterVisible,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                FromDate = message.FromDate,
                ToDate = message.ToDate,

                CustId = message.CustId,
                CustName = message.CustName,
                CustLevelId = message.CustLevelId,
                BreadcrumbLevel = message.BreadcrumbLevel,

                ReportData = reportData,
                IsPostBack = message.IsPostBack
            };

            return ExcelView(query);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!viewer.IsShowConsolidatedReport)
            {
                UnAuthentication(filterContext);
            }
        }
    }
}