namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using Params;
    using Reports.Models;
    using Requests.WinLossDetail;
    using Services;

    public class WinLossDetailBaseHandler : BaseHandler
    {
        [SuppressMessage(
            "StyleCop.CSharp.NamingRules",
            "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        protected readonly ProductService productService;

        [SuppressMessage(
            "StyleCop.CSharp.NamingRules",
            "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        protected readonly WinLossDetailService reportService;

        public WinLossDetailBaseHandler(
            ProductService productService,
            WinLossDetailService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
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