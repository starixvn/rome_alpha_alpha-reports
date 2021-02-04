namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Params;
    using Alpha.Reports.Site.Requests.WinLossDetail;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params.Consolidation;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Requests.Consolidation;
    using Services;
    using ViewModels;
    using ViewModels.Consolidation;
    using static ONEbook.SpreadSheets.Column;

    public class ConsolidationExcelHandler :
        ConsolidationBaseHandler,
        IRequestHandler<ConsolidationByUsernameExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ConsolidationByDateExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ConsolidationByProductExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ConsolidationDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<ConsolidationByUsernameViewModel> workbook;
        private readonly Workbook<WinLossDetailModel> workbookForDetail;
        private readonly ConsolidationByDateWorkbook consolidationByDateWorkbook;
        private readonly ConsolidationByProductWorkbook consolidationByProductWorkbook;
        protected readonly ProductService productService;

        public ConsolidationExcelHandler(
            ProductService productService,
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            IUser viewer,
            Workbook<ConsolidationByUsernameViewModel> workbook,
            Workbook<WinLossDetailModel> workbookForDetail,
            ConsolidationByDateWorkbook consolidationByDateWorkbook,
            ConsolidationByProductWorkbook consolidationByProductWorkbook,
            ProductDetailService productDetailService) :
            base(reportService, cacheRefService, viewer, productDetailService)
        {
            this.workbook = workbook;
            this.workbookForDetail = workbookForDetail;
            this.consolidationByDateWorkbook = consolidationByDateWorkbook;
            this.consolidationByProductWorkbook = consolidationByProductWorkbook;
            this.productService = productService;
        }

        public ExcelFileViewModel Handle(ConsolidationByUsernameExcelQuery message)
        {
            var consolidationExcludeProduct = reportService.GetConsolidationExcludeProduct(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var reportData = reportService.GetConsolidationByUsername(param);
            var currencies = reportService
                .GetCurrencies(new GetCurrencyParam(message.ViewerId))
                .ToDictionary(c => c.Id, c => c.Name);
            var viewModels = BuildWinLossByUsernameViewModel(reportData, currencies);

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo("ConsolidationByUsername"))
                .Title(GetReporTitle(param, Resources.ConsolidationReportByUsername, currencies))
                .Header(Resources.ResourceManager.GetString)
                .Body(viewModels, GetRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), fileContent);
        }

        public ExcelFileViewModel Handle(ConsolidationByDateExcelQuery message)
        {
            var consolidationExcludeProduct = reportService.GetConsolidationExcludeProduct(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var reportData = reportService.GetConsolidationByDate(param);
            var currencies = reportService
                .GetCurrencies(new GetCurrencyParam(message.ViewerId))
                .ToDictionary(c => c.Id, c => c.Name);
            var viewModels = BuildWinLossByDateViewModel(reportData, currencies);

            var fileContent = consolidationByDateWorkbook
                     .UseTemplate(GetTemplateInfo("ConsolidationByDate"))
                     .Title(GetReporTitle(param, Resources.ConsolidationReportByDate, currencies))
                     .Header(Resources.ResourceManager.GetString)
                     .Body(viewModels, null)
                     .Sheet(Resources.DefaultSheetName)
                     .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), fileContent);
        }

        public ExcelFileViewModel Handle(ConsolidationByProductExcelQuery message)
        {
            var consolidationExcludeProduct = reportService.GetConsolidationExcludeProduct(message.CustId, message.MinDate, message.IsEnableSabaPromotionForHeadline);

            var param = CreateParameter(message, consolidationExcludeProduct.Products);
            var reportData = reportService.GetConsolidationByProduct(param);
            var currencies = reportService
                .GetCurrencies(new GetCurrencyParam(message.ViewerId))
                .ToDictionary(c => c.Id, c => c.Name);
            var viewModels = BuildWinLossByProductViewModel(reportData, currencies);

            var fileContent = consolidationByProductWorkbook
                     .UseTemplate(GetTemplateInfo("ConsolidationByProduct"))
                     .Title(GetReporTitle(param, Resources.ConsolidationReportByProduct, currencies))
                     .Header(Resources.ResourceManager.GetString)
                     .Body(viewModels, null)
                     .Sheet(Resources.DefaultSheetName)
                     .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), fileContent);
        }

        public ExcelFileViewModel Handle(ConsolidationDetailExcelQuery message)
        {
            var winLossDetailQuery = CreateWinLossDetailQuery(message);

            var products = reportService.GetProducts(
                new GetConslidationReportProductsParam(winLossDetailQuery.ViewerId, winLossDetailQuery.MinDate),
                message.IsEnableSabaPromotionForHeadline);
            var param = CreateWinLossDetailParam(winLossDetailQuery, products);

            var reportData = reportService.GetWinLossDetail(param);

            var fileContent = workbookForDetail
               .UseTemplate(GetTemplateInfo(message.CustLevel.GetDirectDownLineLevel().DisplayName))
               .Title(GetConsolidationReporTitle(param))
               .Header(Resources.ResourceManager.GetString)
               .Body(reportData.DataSource, GetBodyRowMapperForDetail(message))
               .Footer(reportData.TotalData, GetTotalRowMapperForDetail(message))
               .Sheet(Resources.DefaultSheetName)
               .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileNameForDetail(message, param), fileContent);
        }

        private TemplateInfo GetTemplateInfo(string templateName)
            => new TemplateInfo(
                GetExcelTemplatePath(templateName),
                noInfomationText: Resources.NoInformationIsAvailable)
            .FreezePanel(4, C);

        private string GetConsolidationReporTitle(WinLossDetailParam param)
        {
            var replacedProductNames = productDetailService.GetProductNamesExportExcel(param.UserSelectedResourceIds);
            var productNames = productService.ConcatProductNames(param.UserSelectedProductIds, replacedProductNames);

            return $"{Resources.WinLossDetail} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()} {Resources.of2} {productNames}";
        }

        private string GetReporTitle(GetConsolidationParam param, string reportType, IDictionary<byte, string> currencies)
        {
            var replacedProductNames = productDetailService.GetProductNamesExportExcel(param.ResourceIds);
            var productNames = productService.ConcatProductNames(param.ProductIds, replacedProductNames);

            var currencyName = param.Currency == DefaultVariables.AllCurrencies
                ? Resources.AllCurrencies
                : Resources.Currency + ": " + currencies[(byte)param.Currency];

            return string.Join(
                " ",
                reportType,
                param.FromDate.ToShortDateWithSlash(),
                "-->",
                param.ToDate.ToShortDateWithSlash(),
                Resources.of2,
                productNames,
                "-",
                currencyName);
        }

        internal Mapper<ConsolidationByUsernameViewModel> GetRowMapper()
        {
            var mapper = new Mapper<ConsolidationByUsernameViewModel>()
                .Map(A, winLoss => winLoss.CustLevelName)
                .Map(B, winLoss => winLoss.Username)
                .Map(C, winLoss => winLoss.CurrencyName)
                .Map(D, winLoss => winLoss.Turnover)
                .Map(E, winLoss => winLoss.GrossComm)
                .Map(F, winLoss => winLoss.MemberWinLoss)
                .Map(G, winLoss => winLoss.MemberComm)
                .Map(H, winLoss => winLoss.MemberTotal)
                .Map(I, winLoss => winLoss.OwnerTurnover)
                .Map(J, winLoss => winLoss.OwnerWinLoss)
                .Map(K, winLoss => winLoss.OwnerComm)
                .Map(L, winLoss => winLoss.OwnerTotal)
                .Map(M, winLoss => winLoss.Company)
                .Map(N, winLoss => winLoss.BetCount);

            return mapper;
        }

        private string GetExcelFileName(GetConsolidationParam param)
            => string.Join(
                "-",
                Resources.Consolidation,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());

        private string GetExcelFileNameForDetail(ConsolidationDetailExcelQuery message, WinLossDetailParam winLossDetailParam)
            => string.Join(
                "-",
                Resources.WinLossDetail,
                message.CustName,
                winLossDetailParam.FromDate.ToShortDateWithSlash(),
                winLossDetailParam.ToDate.ToShortDateWithSlash());

        private Mapper<WinLossDetailModel> GetBodyRowMapperForDetail(ConsolidationDetailExcelQuery message)
            => GetRowMapperForDetail(message, isTotal: false);

        private Mapper<WinLossDetailModel> GetTotalRowMapperForDetail(ConsolidationDetailExcelQuery message)
            => GetRowMapperForDetail(message, isTotal: true);

        internal Mapper<WinLossDetailModel> GetRowMapperForDetail(ConsolidationDetailExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossDetailModel>()
                .Map(A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(B, winLossDetail => winLossDetail.BetCount)
                .Map(C, winLossDetail => winLossDetail.Turnover)
                .Map(D, winLossDetail => winLossDetail.NetTurnover)
                .Map(E, winLossDetail => winLossDetail.GrossComm)
                .Map(F, winLossDetail => winLossDetail.MemberNetWinLoss.WinLoss)
                .Map(G, winLossDetail => winLossDetail.MemberNetWinLoss.Comm)
                .Map(H, winLossDetail => winLossDetail.MemberNetWinLoss.Total)
                .Map(I, winLossDetail => winLossDetail.AgentNetWinLoss.WinLoss)
                .Map(J, winLossDetail => winLossDetail.AgentNetWinLoss.Comm)
                .Map(K, winLossDetail => winLossDetail.AgentNetWinLoss.Total);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(L, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(L, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(M, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(N, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(O, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper
                    .Map(O, winLossDetail => winLossDetail.SuperNetWinLoss.WinLoss)
                    .Map(P, winLossDetail => winLossDetail.SuperNetWinLoss.Comm)
                    .Map(Q, winLossDetail => winLossDetail.SuperNetWinLoss.Total)
                    .Map(R, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }
    }
}