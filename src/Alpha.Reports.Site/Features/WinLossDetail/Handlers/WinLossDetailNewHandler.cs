namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.WinLossDetail;
    using Alpha.Reports.Params;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailNewHandler : WinLossDetailBaseNewHandler,
        IAsyncRequestHandler<WinLossDetailNewQuery, WinLossDetailNewViewModel>,
        IAsyncRequestHandler<WinLossDetailGetDataQuery, WinLossDetailGetDataViewModel>,
        IAsyncRequestHandler<WinLossDetailGetDataWeekQuery, WinLossDetailGetDataWeekViewModel>,
        IAsyncRequestHandler<WinLossDetailWeeksQuery, IEnumerable<WeekSelectionViewModel>>
    {
        private readonly WinLossService winlossService;

        public WinLossDetailNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossService winlossService,
            WinLossDetailService winlossDetailService,
            CustomerService customerService) : base(productService, productDetailService, winlossDetailService, customerService)
        {
            this.winlossService = winlossService;
        }

        public async Task<WinLossDetailNewViewModel> Handle(WinLossDetailNewQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var isValidUsername = BuildWinLossDetailGetDataQuery(message);
            Task<PagedReportData<WinLossDetailModel>> reportDataTask;
            Task<IEnumerable<WeekSelectionViewModel>> weekDataTask;

            if (isValidUsername)
            {
                var winLossDetailParam = CreateParameter(message, products, productDetails);
                reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);

                var winLossDetailWeekDataParam = new WinLossDetailWeekDataParam(
                    winLossDetailParam.ViewerId,
                    message.FromDate,
                    message.ToDate,
                    winLossDetailParam.LimitedDateToShowInfo,
                    winLossDetailParam.UserSelectedProductIds,
                    winLossDetailParam.ProductDetailsXML,
                    string.IsNullOrWhiteSpace(winLossDetailParam.Username) ? message.CustName : winLossDetailParam.Username,
                    isReward: true,
                    site: message.Site);
                weekDataTask = BuildWeekSelectionViewModels(winLossDetailWeekDataParam, message.WeekFromDate, message.WeekToDate);
            }
            else
            {
                reportDataTask = Task.FromResult(
                    new PagedReportData<WinLossDetailModel>(null, message.PageIndex, message.PageSize, 0, null));
                weekDataTask = Task.FromResult(Enumerable.Empty<WeekSelectionViewModel>());
            }

            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, weekDataTask, totalEnteredSportTask);

            return new WinLossDetailNewViewModel(
                message,
                BuildProductViewModels(message, products),
                BuildProductDetailNewViewModels(message, productDetails),
                await totalEnteredSportTask,
                await reportDataTask,
                await weekDataTask);
        }

        public async Task<WinLossDetailGetDataViewModel> Handle(WinLossDetailGetDataQuery message)
        {
            var isValidUsername = BuildWinLossDetailGetDataQuery(message);
            Task<PagedReportData<WinLossDetailModel>> reportDataTask;

            if (isValidUsername)
            {
                var winLossDetailParam = await CreateParameter(message);
                reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
            }
            else
            {
                reportDataTask = Task.FromResult(
                    new PagedReportData<WinLossDetailModel>(null, message.PageIndex, message.PageSize, 0, null));
            }

            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask);

            return new WinLossDetailGetDataViewModel(
                message,
                await totalEnteredSportTask,
                await reportDataTask);
        }

        public async Task<WinLossDetailGetDataWeekViewModel> Handle(WinLossDetailGetDataWeekQuery message)
        {
            var isValidUsername = BuildWinLossDetailGetDataQuery(message);
            Task<PagedReportData<WinLossDetailModel>> reportDataTask;

            if (isValidUsername)
            {
                var winLossDetailParam = await CreateParameter(message);
                reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);
            }
            else
            {
                reportDataTask = Task.FromResult(
                    new PagedReportData<WinLossDetailModel>(null, message.PageIndex, message.PageSize, 0, null));
            }

            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask);

            return new WinLossDetailGetDataWeekViewModel(message, await totalEnteredSportTask, await reportDataTask);
        }

        public async Task<IEnumerable<WeekSelectionViewModel>> Handle(WinLossDetailWeeksQuery message)
        {
            var isValidUsername = BuildWinLossDetailGetDataQuery(message);

            if (!isValidUsername)
            {
                return Enumerable.Empty<WeekSelectionViewModel>();
            }

            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;
            var winLossDetailParam = CreateParameter(message, products, productDetails);

            var winLossDetailWeekDataParam = new WinLossDetailWeekDataParam(
                winLossDetailParam.ViewerId,
                message.FromDate,
                message.ToDate,
                winLossDetailParam.LimitedDateToShowInfo,
                winLossDetailParam.UserSelectedProductIds,
                winLossDetailParam.ProductDetailsXML,
                string.IsNullOrWhiteSpace(winLossDetailParam.Username) ? message.CustName : winLossDetailParam.Username,
                isReward: true,
                site: message.Site);

            return await BuildWeekSelectionViewModels(winLossDetailWeekDataParam, message.WeekFromDate, message.WeekToDate);
        }

        private async Task<IEnumerable<WeekSelectionViewModel>> BuildWeekSelectionViewModels(
            WinLossDetailWeekDataParam param,
            DateTime? selectedFromDate,
            DateTime? selectedToDate)
        {
            var weekData = await winlossDetailService.GetWinLossDetailWeekDataAsync(param);
            var weekDataHash = new HashSet<string>();
            foreach (var item in weekData)
            {
                weekDataHash.Add(GetDisplayWeek(item.StartOfWeek, item.EndOfWeek));
            }

            var weekSelections = new List<WeekSelectionViewModel>();
            var startDate = param.FromDate.Date;
            var endDate = param.ToDate.Date;

            while (endDate.Subtract(startDate).TotalDays >= 0)
            {
                var endDateStartOfWeek = endDate.StartOfWeek();
                var startDateOrEndDateStartOfWeek = startDate < endDateStartOfWeek ? endDateStartOfWeek : startDate;
                var weekSelection = new WeekSelectionViewModel(
                    startDateOrEndDateStartOfWeek,
                    endDate,
                    GetDisplayWeek(startDateOrEndDateStartOfWeek, endDate),
                    selectedFromDate,
                    selectedToDate);
                weekSelection.IsEnable = weekDataHash.Contains(weekSelection.Display);
                weekSelections.Add(weekSelection);

                endDate = endDateStartOfWeek.AddDays(-1);
            }

            return weekSelections;
        }

        private static string GetDisplayWeek(DateTime fromDate, DateTime toDate)
        {
            var displayToDate = $"{toDate.ToString("dd")} {Resources.ResourceManager.GetString($"_{toDate.ToString("MMM")}")}";

            return fromDate == toDate ? displayToDate : $"{fromDate.ToString("dd")} - {displayToDate}";
        }
    }
}