namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Reports.Excel.Services;
    using Alpha.Reports.Site.Requests.WinLoss;
    using Enumerations;
    using Excel.Params;
    using MediatR;
    using Models;
    using Models.Winloss;
    using Newtonsoft.Json;
    using Params;
    using Reports.Extensions;
    using Requests.WinLossDetails;
    using Services;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        BaseHandler,
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<EnterResultQuery, EnteredResultViewModel>,
        IRequestHandler<ExportWinLossQuery, byte[]>,
        IRequestHandler<ExportEnteredResultQuery, byte[]>
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly WinLossExcelExporter excelExporter;

        public WinLossHandler(
            ProductService productService,
            WinLossService reportService,
            WinLossExcelExporter excelExporter)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.excelExporter = excelExporter;
        }

        public EnteredResultViewModel Handle(EnterResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            var reportData = reportService.GetEnteredSportResults();

            var viewModel = new EnteredResultViewModel
            {
                UserLevel = userLevel,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                Language = message.Language,
                ReportData = reportData,
                UserSelectedDateRangeOption = 0,
                SortPropertyName = "SportName"
            };

            return viewModel;
        }

        public WinLossViewModel Handle(WinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossParam = CreateWinLossParamater(message, userLevel);

            var reportData = reportService.GetWinLoss(winLossParam);

            var viewModel = new WinLossViewModel
            {
                CustId = winLossParam.CustId,
                CustName = message.CustName,
                UserLevel = userLevel,
                FromDate = winLossParam.FromDate,
                ToDate = winLossParam.ToDate,

                Products = BuildProductSelectListItems(winLossParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                ReportData = reportData,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                IsDirectReportViewAction = message.IsDirectReportViewAction,

                SortPropertyName = winLossParam.SortByPropertyName,
                IsDescendingSort = winLossParam.IsDescendingSort,
                HasSubmittedParams = message.HasSubmittedParams,
                BreadcrumbLevels = BuildBreadcrumbLevels(
                    message,
                    (custInfo, query, breadcrumbLevels) => new WinLossQuery
                    {
                        CustId = custInfo.CustId,
                        CustName = custInfo.CustName,
                        PageSize = message.PageSize,
                        FromDate = message.FromDate,
                        ToDate = message.ToDate,

                        UserSelectedProductIds = message.UserSelectedProductIds,
                        SortPropertyName = message.SortPropertyName,
                        IsDescendingSort = message.IsDescendingSort,
                        IsStackMode = message.IsStackMode,
                        IsHistoricReport = message.IsHistoricReport,
                        IsFilterVisible = message.IsFilterVisible,
                        UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                        UserLevelId = custInfo.UserLevelId,
                        BreadcrumbLevels = breadcrumbLevels
                    })
            };

            return viewModel;
        }

        public byte[] Handle(ExportWinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var fileContents = excelExporter.ExportWinLossExcel(
                new ExportExcelParam<WinLoss>
                {
                    TemplatePath = message.TemplatePath,
                    ReportName = message.Title,
                    SheetName = message.Title,
                    ReportData = message.ReportData,
                    FromDate = message.FromDate,
                    ToDate = message.ToDate
                },
                new ExportWinLossParam(message.UserLevel, message.UserSelectedProductIds));

            return fileContents;
        }

        public byte[] Handle(ExportEnteredResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var fileContents = excelExporter.ExportEnteredResultExcel(
                new ExportExcelParam<EnteredSportResult>
                {
                    TemplatePath = message.TemplatePath,
                    ReportName = message.Title,
                    SheetName = message.Title,
                    ReportData = message.ReportData,
                });

            return fileContents;
        }

        private WinLossParam CreateWinLossParamater(
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
                 message.SortPropertyName,
                 message.IsDescendingSort,
                 message.IsHistoricReport);
        }
    }
}