namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using Models;
    using Params;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;

    public class BaseWinLossDetailHandler : BaseHandler
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;

        public BaseWinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
        }

        protected ProductService ProductService
        {
            get
            {
                return productService;
            }
        }

        protected WinLossDetailService ReportService
        {
            get
            {
                return reportService;
            }
        }

        protected WinLossDetailParam CreateParameter(
            WinLossDetailQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

            return new WinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 userLevel,
                 message.SortPropertyName,
                 message.IsDescendingSort,
                 message.IsHistoricReport);
        }

        protected string GetReportTitle(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return Resources.MasterWinLossDetail;
            }

            if (userLevel.IsMasterLevel())
            {
                return Resources.AgentWinLossDetail;
            }

            return Resources.MemberWinLossDetail;
        }
    }
}