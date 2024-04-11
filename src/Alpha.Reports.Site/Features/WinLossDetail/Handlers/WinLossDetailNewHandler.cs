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
        IAsyncRequestHandler<WinLossDetailGetDataWeekQuery, WinLossDetailGetDataWeekViewModel>
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
            Task<IEnumerable<WinLossDetailWeekData>> weekDataTask;

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
                    string.IsNullOrWhiteSpace(winLossDetailParam.Username) ? message.CustName : winLossDetailParam.Username);
                weekDataTask = winlossDetailService.GetWinLossDetailWeekDataAsync(winLossDetailWeekDataParam);
            }
            else
            {
                reportDataTask = Task.FromResult(
                    new PagedReportData<WinLossDetailModel>(null, message.PageIndex, message.PageSize, 0, null));
                weekDataTask = Task.FromResult(Enumerable.Empty<WinLossDetailWeekData>());
            }
            
            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, weekDataTask, totalEnteredSportTask);

            return new WinLossDetailNewViewModel(
                message,
                BuildProductViewModels(message, products),
                BuildProductDetailNewViewModels(message, productDetails),
                await totalEnteredSportTask,
                await reportDataTask,
                BuildWeekSelectionViewModels(
                    await weekDataTask,
                    message.FromDate,
                    message.ToDate,
                    message.WeekFromDate,
                    message.WeekToDate));
        }

        public async Task<WinLossDetailGetDataViewModel> Handle(WinLossDetailGetDataQuery message)
        {
            var isValidUsername = BuildWinLossDetailGetDataQuery(message);
            Task<PagedReportData<WinLossDetailModel>> reportDataTask;
            Task<IEnumerable<WinLossDetailWeekData>> weekDataTask;

            if (isValidUsername)
            {
                var winLossDetailParam = await CreateParameter(message);
                reportDataTask = winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);

                var winLossDetailWeekDataParam = new WinLossDetailWeekDataParam(
                    winLossDetailParam.ViewerId,
                    message.FromDate,
                    message.ToDate,
                    winLossDetailParam.LimitedDateToShowInfo,
                    winLossDetailParam.UserSelectedProductIds,
                    winLossDetailParam.ProductDetailsXML,
                    string.IsNullOrWhiteSpace(winLossDetailParam.Username) ? message.CustName : winLossDetailParam.Username);
                weekDataTask = winlossDetailService.GetWinLossDetailWeekDataAsync(winLossDetailWeekDataParam);
            }
            else
            {
                reportDataTask = Task.FromResult(
                    new PagedReportData<WinLossDetailModel>(null, message.PageIndex, message.PageSize, 0, null));
                weekDataTask = Task.FromResult(Enumerable.Empty<WinLossDetailWeekData>());
            }

            var totalEnteredSportTask = winlossService.GetTotalEnteredSportYesterdayAndTodayAsync();
            await Task.WhenAll(reportDataTask, totalEnteredSportTask, weekDataTask);

            return new WinLossDetailGetDataViewModel(
                message,
                await totalEnteredSportTask,
                await reportDataTask,
                BuildWeekSelectionViewModels(
                    await weekDataTask,
                    message.FromDate,
                    message.ToDate,
                    message.WeekFromDate,
                    message.WeekToDate));
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

        private static IEnumerable<WeekSelectionViewModel> BuildWeekSelectionViewModels(
            IEnumerable<WinLossDetailWeekData> weekData,
            DateTime fromDate,
            DateTime toDate,
            DateTime? selectedFromDate = null,
            DateTime? selectedToDate = null)
        {
            var weekDataHash = new HashSet<string>();
            foreach (var item in weekData)
            {
                weekDataHash.Add(GetDisplayWeek(item.StartOfWeek, item.EndOfWeek));
            }

            var weekSelections = new List<WeekSelectionViewModel>();
            var startDate = fromDate.Date;
            var endDate = toDate.Date;

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