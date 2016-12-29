namespace Alpha.Reports.Site.Handlers.WinLoss
{
    using System.Linq;
    using Extensions;
    using Models;
    using Params;
    using Requests.WinLoss;
    using Services;

    public class BaseWinLossHandler : BaseHandler
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;

        public BaseWinLossHandler(
            ProductService productService,
            WinLossService reportService)
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

        protected WinLossService ReportService
        {
            get
            {
                return reportService;
            }
        }

        protected WinLossParam CreateWinLossParamater(
             WinLossQuery message,
             UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

            return new WinLossParam(
                 message.CustId,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 userLevel,
                 message.IsHistoricReport);
        }
    }
}