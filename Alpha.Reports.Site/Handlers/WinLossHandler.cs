namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Excel.Services;
    using Requests.WinLoss;
    using Enumerations;
    using Excel.Params;
    using MediatR;
    using Models;
    using Newtonsoft.Json;
    using Params;
    using Extensions;
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
                UserSelectedDateRangeOption = 0
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

            var viewModel = new WinLossViewModel(reportData.ToList())
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

                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                IsDirectReportViewAction = message.IsDirectReportViewAction,
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

            var fileContents = excelExporter.ExportWinLossExcel(
                new ExportWinLossParam(
                    templatePath: message.TemplatePath,
                    sheetName: message.Title,
                    reportName: message.Title,
                    reportData: message.ReportData,
                    grantTotal: message.GrantTotal,
                    fromDate: message.FromDate,
                    toDate: message.ToDate,
                    userLevel: message.UserLevel,
                    userSelectedProductIds: message.UserSelectedProductIds));

            return fileContents;
        }

        public byte[] Handle(ExportEnteredResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var fileContents = excelExporter.ExportEnteredResultExcel(
                new ExportEnteredResultParam(
                    templatePath: message.TemplatePath,
                    reportName: message.Title,
                    sheetName: message.Title,
                    reportData: message.ReportData));

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
                 message.IsHistoricReport);
        }

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
                            FromDate = message.FromDate,
                            ToDate = message.ToDate,

                            UserSelectedProductIds = message.UserSelectedProductIds,
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
                    FromDate = message.FromDate,
                    ToDate = message.ToDate,
                    UserSelectedProductIds = message.UserSelectedProductIds,

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