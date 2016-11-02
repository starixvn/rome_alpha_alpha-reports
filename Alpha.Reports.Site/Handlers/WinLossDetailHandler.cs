namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using Params;
    using Reports.AppResources;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        IRequestHandler<WinLossDetailQuery<MasterWinLossDetail>, WinLossDetailViewModel<MasterWinLossDetail>>,
        IRequestHandler<WinLossDetailQuery<AgentWinLossDetail>, WinLossDetailViewModel<AgentWinLossDetail>>,
        IRequestHandler<WinLossDetailQuery<MemberWinLossDetail>, WinLossDetailViewModel<MemberWinLossDetail>>
    {
        private const int AllBetProductId = 21;
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;
        private readonly ApplicationContext applicationContext;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            ApplicationContext applicationContext)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.applicationContext = applicationContext;
        }

        public WinLossDetailViewModel<MasterWinLossDetail> Handle(WinLossDetailQuery<MasterWinLossDetail> message)
        {
            return HandleRequest<MasterWinLossDetail, WinLossDetailViewModel<MasterWinLossDetail>>(message);
        }

        public WinLossDetailViewModel<AgentWinLossDetail> Handle(WinLossDetailQuery<AgentWinLossDetail> message)
        {
            return HandleRequest<AgentWinLossDetail, WinLossDetailViewModel<AgentWinLossDetail>>(message);
        }

        public WinLossDetailViewModel<MemberWinLossDetail> Handle(WinLossDetailQuery<MemberWinLossDetail> message)
        {
            return HandleRequest<MemberWinLossDetail, WinLossDetailViewModel<MemberWinLossDetail>>(message);
        }

        private TViewModel HandleRequest<TModel, TViewModel>(WinLossDetailQuery<TModel> message)
            where TModel : MemberWinLossDetail
            where TViewModel : WinLossDetailViewModel<TModel>, new()
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId);

            var param = CreateWinLossDetailParam(message, products);
            var reportData = reportService.GetWinLossDetail<TModel>(param);

            var netTurnoverTooltips = GetNetTurnoverTooltip(products);

            var viewModel = new TViewModel
            {
                CustId = param.CustId,
                CurrentDate = DateTime.Now,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = products,
                SelectedProducts = string.Join(",", param.ProductIds),
                Language = message.Language,

                NetTurnoverTooltips = netTurnoverTooltips,

                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = applicationContext.OldMainAppPath,

                ReportData = reportData,
                SelectedDateRange = message.SelectedDateRange
            };

            return viewModel;
        }

        private WinLossDetailParam CreateWinLossDetailParam<TModel>(
            WinLossDetailQuery<TModel> message,
            IEnumerable<Product> userProducts) where TModel : MemberWinLossDetail
        {
            var selectedProducts = string.IsNullOrWhiteSpace(message.SelectedProducts)
                ? string.Join(",", userProducts.Select(product => product.Id))
                : message.SelectedProducts;

            return new WinLossDetailParam(
                message.CustId,
                message.PageIndex,
                message.PageSize,
                message.FromDate,
                message.ToDate,
                selectedProducts,
                message.UserLevel,
                message.SortedColumn,
                message.SortDirection,
                message.IsHistoricReport);
        }

        private List<string> GetNetTurnoverTooltip(IEnumerable<Product> products)
        {
            var netTurnoverTooltips = new List<string>();

            if (products.Where(product => product.Id == AllBetProductId) != null)
            {
                netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
            }

            return netTurnoverTooltips;
        }
    }
}