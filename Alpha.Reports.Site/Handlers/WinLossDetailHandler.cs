namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;
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
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId.Value);

            var param = CreateWinLossDetailParam(message, products);
            var reportData = reportService.GetWinLossDetail<MasterWinLossDetail>(param);

            var reportRange = dateService.GetDateRangeLimit(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = GetNetTurnoverTooltip(products);

            return new MasterWinLossDetailViewModel
            {
                MinDate = reportRange.MinDate,
                MaxDate = reportRange.MaxDate,
                CurrentDate = DateTime.Now,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = JsonConvert.SerializeObject(products),
                SelectedProducts = string.Join(",", param.ProductIds),
                Language = message.Language,

                NetTurnoverTooltips = netTurnoverTooltips,
                ReportData = reportData,

                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = message.OldMainAppPath
            };
        }

        public AgentWinLossDetailViewModel Handle(AgentWinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId.Value);

            var param = CreateWinLossDetailParam(message, products);
            var reportData = reportService.GetWinLossDetail<AgentWinLossDetail>(param);

            var reportRange = dateService.GetDateRangeLimit(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = GetNetTurnoverTooltip(products);

            return new AgentWinLossDetailViewModel
            {
                MinDate = reportRange.MinDate,
                MaxDate = reportRange.MaxDate,
                CurrentDate = DateTime.Now,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = JsonConvert.SerializeObject(products),
                SelectedProducts = string.Join(",", param.ProductIds),
                Language = message.Language,

                NetTurnoverTooltips = netTurnoverTooltips,
                ReportData = reportData,

                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = message.OldMainAppPath
            };
        }

        public MemberWinLossDetailViewModel Handle(MemberWinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId.Value);

            var param = CreateWinLossDetailParam(message, products);
            var reportData = reportService.GetWinLossDetail<MemberWinLossDetail>(param);

            var reportRange = dateService.GetDateRangeLimit(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = GetNetTurnoverTooltip(products);

            return new MemberWinLossDetailViewModel
            {
                MinDate = reportRange.MinDate,
                MaxDate = reportRange.MaxDate,
                CurrentDate = DateTime.Now,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = JsonConvert.SerializeObject(products),
                SelectedProducts = string.Join(",", param.ProductIds),
                Language = message.Language,

                NetTurnoverTooltips = netTurnoverTooltips,
                ReportData = reportData,

                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = message.OldMainAppPath
            };
        }

        private WinLossDetailParam CreateWinLossDetailParam(
            WinLossDetailQuery message,
            IEnumerable<Product> visibleProducts)
        {
            message.SelectedProducts = "1,2";
            message.FromDate = siteContext.ServerTime.AddMonths(-3).FirstDayOfMonth();

            var selectedProducts = string.IsNullOrWhiteSpace(message.SelectedProducts)
                ? visibleProducts.Select(product => product.Id).ToList()
                : message.SelectedProducts.Split(',').Select(product => Converter.ToInt(product)).ToList();

            return new WinLossDetailParam(
                message.CustId.Value,
                message.PageIndex ?? DefaultVariables.PageIndex,
                message.PageSize ?? DefaultVariables.PageSize,
                message.FromDate ?? siteContext.ServerTime,
                message.ToDate ?? siteContext.ServerTime,
                message.SelectedProducts, 
                message.UserLevel);
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