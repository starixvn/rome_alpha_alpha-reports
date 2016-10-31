namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Constants;
    using Extensions;
    using Fanex.Common;
    using MediatR;
    using Models;
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
        private readonly ProductService productService;
        private readonly ReportDateService reportDateService;
        private readonly WinLossDetailService winLossDetailService;
        private readonly SiteContext siteContext;

        public WinLossDetailHandler(
            ProductService productService,
            ReportDateService reportDateService,
            WinLossDetailService winLossDetailService,
            SiteContext siteContext)
        {
            this.productService = productService;
            this.reportDateService = reportDateService;
            this.winLossDetailService = winLossDetailService;
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
            var reportData = winLossDetailService.MasterWinLossDetail(param);

            var reportRange = reportDateService.GetReportDateRange(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = new List<string>();

            if (products.Where(product => product.Id == ProductId.AllBet) != null)
            {
                netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
            }

            return new MasterWinLossDetailViewModel
            {
                MinDate = reportRange.FromDate,
                MaxDate = reportRange.ToDate,
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
            var reportData = winLossDetailService.AgentWinLossDetail(param);

            var reportRange = reportDateService.GetReportDateRange(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = new List<string>();

            if (products.Where(product => product.Id == ProductId.AllBet) != null)
            {
                netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
            }

            return new AgentWinLossDetailViewModel
            {
                MinDate = reportRange.FromDate,
                MaxDate = reportRange.ToDate,
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
            var reportData = winLossDetailService.MemberWinLossDetail(param);

            var reportRange = reportDateService.GetReportDateRange(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                message.ReportRanges));

            var netTurnoverTooltips = new List<string>();

            if (products.Where(product => product.Id == ProductId.AllBet) != null)
            {
                netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
            }

            return new MemberWinLossDetailViewModel
            {
                MinDate = reportRange.FromDate,
                MaxDate = reportRange.ToDate,
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
            var selectedProducts = string.IsNullOrWhiteSpace(message.SelectedProducts)
                ? visibleProducts.Select(product => product.Id).ToList()
                : message.SelectedProducts.Split(',').Select(product => Converter.ToInt(product)).ToList();

            return new WinLossDetailParam(
                message.CustId.Value,
                message.FromDate ?? siteContext.ServerTime.AddMonths(-3),
                message.ToDate ?? siteContext.ServerTime,
                selectedProducts,
                message.PageSize ?? DefaultVariables.PageSize,
                message.PageIndex ?? DefaultVariables.PageIndex);
        }
    }
}