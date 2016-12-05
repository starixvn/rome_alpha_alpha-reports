namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Enumerations;
    using Excel.Params;
    using Excel.Services;
    using Extensions;
    using MediatR;
    using Models;
    using Models.WinlossDetails;
    using Newtonsoft.Json;
    using Params;
    using Requests.WinLossDetails;
    using Services;
    using ViewModels;
    using ViewModels.WinLossDetails;

    public class WinLossDetailHandler :
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<ExportWinLossDetailQuery, byte[]>
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;
        private readonly WinLossDetailExcelExporter excelExporter;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            WinLossDetailExcelExporter excelExporter)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.excelExporter = excelExporter;
        }

        public WinLossDetailViewModel Handle(WinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var userLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);
            var products = productService.GetProductsForCustomer(message.CustId);

            var winLossDetailParam = CreateParamater(message, userLevel);

            var reportData = reportService.GetWinLossDetail(winLossDetailParam);

            var viewModel = new WinLossDetailViewModel
            {
                CustId = winLossDetailParam.CustId,
                CustName = message.CustName,
                UserLevel = userLevel,
                FromDate = winLossDetailParam.FromDate,
                ToDate = winLossDetailParam.ToDate,

                Products = BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossDetailParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                ReportData = reportData,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                IsDirectReportViewAction = message.IsDirectReportViewAction,

                SortPropertyName = winLossDetailParam.SortByPropertyName,
                IsDescendingSort = winLossDetailParam.IsDescendingSort,
                HasSubmittedParams = message.HasSubmittedParams,
                BreadcrumbLevels = BuildBreadcrumbLevels(message)
            };

            return viewModel;
        }

        public byte[] Handle(ExportWinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var fileContents = excelExporter.Export(
                new ExportExcelParam<WinLossDetail>
                {
                    TemplatePath = message.TemplatePath,
                    ReportName = message.Title,
                    SheetName = message.Title,
                    ReportData = message.ReportData,
                    FromDate = message.FromDate,
                    ToDate = message.ToDate
                },
                new ExportWinLossDetailParam(message.UserLevel, message.UserSelectedProductIds));

            return fileContents;
        }

        private WinLossDetailParam CreateParamater(
            WinLossDetailQuery message,
            UserLevel userLevel)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

            return new WinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
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
        private IEnumerable<ActionLinkViewModel> BuildBreadcrumbLevels(WinLossDetailQuery message)
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
                        controllerName: "WinLossDetail",
                        actionName: "Index",
                        linkText: custInfo.CustName,
                        routeValues: new WinLossDetailQuery
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
                controllerName: "WinLossDetail",
                actionName: "Index",
                linkText: message.CustName,
                routeValues: new WinLossDetailQuery
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