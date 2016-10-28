namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Constants;
    using Fanex.Common;
    using MediatR;
    using Newtonsoft.Json;
    using Params;
    using Reports.AppResources;
    using Reports.Helpers;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        IRequestHandler<MemberWinLossDetailQuery, IndexViewModel>
    {
        private readonly ProductHandler productHandler;
        private readonly ReportDateService reportDateService;
        private readonly WinLossDetailService winLossDetailService;
        private readonly IDateTime dateTimeWrapper;

        public WinLossDetailHandler(
            ProductHandler productHandler,
            ReportDateService reportDateService,
            WinLossDetailService winLossDetailService,
            IDateTime dateTimeWrapper)
        {
            this.productHandler = productHandler;
            this.reportDateService = reportDateService;
            this.winLossDetailService = winLossDetailService;
            this.dateTimeWrapper = dateTimeWrapper;
        }

        public IndexViewModel Handle(MemberWinLossDetailQuery message)
        {
            var reportRange = reportDateService.GetReportDateRange(new ReportDateRangeParam(
                message.ReportPath,
                message.IsHistoricReport,
                AppSettings.ReportRanges));

            var selectedProducts = string.IsNullOrWhiteSpace(message.SelectedProducts)
                ? new List<int>()
                : message.SelectedProducts.Split(',').Select(product => Converter.ToInt(product)).ToList();
            var reportData = winLossDetailService.MemberWinLossDetail(new WinLossDetailParam(
                Converter.ToInt(message.CustId),
                string.IsNullOrWhiteSpace(message.FromDate) ? dateTimeWrapper.Now.AddMonths(-2) : Converter.ToDateTime(message.FromDate),
                string.IsNullOrWhiteSpace(message.ToDate) ? dateTimeWrapper.Now : Converter.ToDateTime(message.ToDate),
                selectedProducts,
                string.IsNullOrWhiteSpace(message.PageSize) ? DefaultVariables.PageSize : Converter.ToInt(message.PageSize),
                string.IsNullOrWhiteSpace(message.PageIndex) ? DefaultVariables.PageIndex : Converter.ToInt(message.PageIndex)));

            var visibleProducts = productHandler.TransformProducts(message.Products);

            var netTurnoverTooltips = new List<string>();
            if (visibleProducts.Where(product => product.Id == ProductId.AllBet) != null)
            {
                netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
            }

            return new IndexViewModel
            {
                MinDate = DateTimeHelper.ToShortDateWithSlash(reportRange.FromDate),
                MaxDate = DateTimeHelper.ToShortDateWithSlash(reportRange.ToDate),
                CurrentDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now),

                Products = JsonConvert.SerializeObject(visibleProducts),
                Language = message.Language,

                NetTurnoverToolTips = netTurnoverTooltips,
                ReportData = reportData
            };
        }
    }
}