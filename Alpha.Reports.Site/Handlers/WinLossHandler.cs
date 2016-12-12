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
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<EnterResultQuery, EnteredResultViewModel>,
        IRequestHandler<ExportWinLossQuery, byte[]>
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
                BreadcrumbLevels = BuildBreadcrumbLevels(message)
            };

            return viewModel;
        }

        public byte[] Handle(ExportWinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var fileContents = excelExporter.Export(
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

        private IEnumerable<SelectListItem> BuildProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
        {
            if (products == null
                || !products.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return products.Select(product =>
            {
                var productSelectListItem = new SelectListItem
                {
                    Text = product.DisplayName,
                    Value = product.Id.ToString(),
                    Selected = userSelectedProductIds.Contains(product.Id)
                };

                return productSelectListItem;
            });
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IEnumerable<ActionLinkViewModel> BuildBreadcrumbLevels(WinLossQuery message)
        {
            var drilldownLevels = new List<ActionLinkViewModel>();
            var breadcrumbLevels = new List<CustInfo>();

            if (!string.IsNullOrWhiteSpace(message.BreadcrumbLevels))
            {
                var messageBreadcrumbLevels = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(message.BreadcrumbLevels.Replace("'", "\""));

                foreach (var custInfo in messageBreadcrumbLevels)
                {
                    if (custInfo.UserLevelId == message.UserLevelId)
                    {
                        break;
                    }

                    breadcrumbLevels.Add(custInfo);

                    var actionLinkViewModel = new ActionLinkViewModel(
                        controllerName: "WinLoss",
                        actionName: "WinLoss",
                        linkText: custInfo.CustName,
                        routeValues: new WinLossQuery
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
                            BreadcrumbLevels = JsonConvert.SerializeObject(breadcrumbLevels).Replace("\"", "'")
                        });

                    drilldownLevels.Add(actionLinkViewModel);
                }
            }

            breadcrumbLevels.Add(new CustInfo(message.CustId, message.CustName, message.UserLevelId));
            drilldownLevels.Add(new ActionLinkViewModel(
                controllerName: "WinLoss",
                actionName: "WinLoss",
                linkText: message.CustName,
                routeValues: new WinLossQuery
                {
                    CustId = message.CustId,
                    CustName = message.CustName,
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
                    UserLevelId = message.UserLevelId,
                    BreadcrumbLevels = JsonConvert.SerializeObject(breadcrumbLevels).Replace("\"", "'")
                }));

            return drilldownLevels;
        }
    }
}