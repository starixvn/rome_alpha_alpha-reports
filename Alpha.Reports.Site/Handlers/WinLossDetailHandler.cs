namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fanex.Common;
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using Newtonsoft.Json;
    using Params;
    using Reports.AppResources;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        IRequestHandler<MasterWinLossDetailQuery, MasterWinLossDetailViewModel>,
        IRequestHandler<AgentWinLossDetailQuery, AgentWinLossDetailViewModel>,
        IRequestHandler<MemberWinLossDetailQuery, MemberWinLossDetailViewModel>
    {
        private const int AllBetProductId = 21;
        private readonly ProductService productService;
        private readonly ReportDateService dateService;
        private readonly WinLossDetailService reportService;
        private readonly ApplicationContext siteContext;

        public WinLossDetailHandler(
            ProductService productService,
            ReportDateService dateService,
            WinLossDetailService reportService,
            ApplicationContext siteContext)
        {
            this.productService = productService;
            this.dateService = dateService;
            this.reportService = reportService;
            this.siteContext = siteContext;
        }

        public MasterWinLossDetailViewModel Handle(MasterWinLossDetailQuery message)
        {
            return HandleRequest<MasterWinLossDetail, MasterWinLossDetailViewModel>(message);
        }

        public AgentWinLossDetailViewModel Handle(AgentWinLossDetailQuery message)
        {
            return HandleRequest<AgentWinLossDetail, AgentWinLossDetailViewModel>(message);
        }

        public MemberWinLossDetailViewModel Handle(MemberWinLossDetailQuery message)
        {
            return HandleRequest<MemberWinLossDetail, MemberWinLossDetailViewModel>(message);
        }

        private TViewModel HandleRequest<TData, TViewModel>(WinLossDetailQuery message)
            where TData : MemberWinLossDetail
            where TViewModel : WinLossDetailViewModel<TData>, new()
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId);

            var param = CreateWinLossDetailParam(message, products);
            var reportData = reportService.GetWinLossDetail<TData>(param);

            var reportRange = dateService.GetDateRangeLimit(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = GetNetTurnoverTooltip(products);

            var viewModel = new TViewModel
            {
                CustId = param.CustId,

                MinDate = reportRange.MinDate,
                MaxDate = reportRange.MaxDate,
                CurrentDate = DateTime.Now,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = JsonConvert.SerializeObject(products),
                SelectedProducts = string.Join(",", param.ProductIds),
                Language = message.Language,

                NetTurnoverTooltips = netTurnoverTooltips,

                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = message.OldMainAppPath,

                ReportData = reportData
            };

            return viewModel;
        }

        private WinLossDetailParam CreateWinLossDetailParam(WinLossDetailQuery message, IEnumerable<Product> userProducts)
        {
            var selectedProducts = string.IsNullOrWhiteSpace(message.SelectedProducts)
                ? string.Join(",", userProducts.Select(product => product.Id))
                : message.SelectedProducts;

            return new WinLossDetailParam(
                message.CustId,
                message.PageIndex ?? DefaultVariables.PageIndex,
                message.PageSize ?? DefaultVariables.PageSize,
                message.FromDate ?? siteContext.ServerTime,
                message.ToDate ?? siteContext.ServerTime,
                selectedProducts,
                message.UserLevel,
                message.SortedColumn,
                message.SortDirection);
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