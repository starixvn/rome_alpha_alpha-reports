namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System.Collections.Generic;
    using System.Linq;
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
        IRequestHandler<ConsolidationDetailExcelQuery, PagedReportData<WinLossDetailModel>>
    {
        private readonly Workbook<ConsolidationByUsernameViewModel> workbook;
        private readonly ConsolidationByDateWorkbook consolidationByDateWorkbook;
        private readonly ConsolidationByProductWorkbook consolidationByProductWorkbook;
        private readonly ProductService productService;

        public ConsolidationExcelHandler(
            ProductService productService,
            ConsolidationService reportService,
            CacheRefService cacheRefService,
            WinLossDetailService winLossDetailService,
            IUser viewer,
            Workbook<ConsolidationByUsernameViewModel> workbook,
            ConsolidationByDateWorkbook consolidationByDateWorkbook,
            ConsolidationByProductWorkbook consolidationByProductWorkbook) :
            base(reportService, cacheRefService, winLossDetailService, viewer)
        {
            this.workbook = workbook;
            this.consolidationByDateWorkbook = consolidationByDateWorkbook;
            this.consolidationByProductWorkbook = consolidationByProductWorkbook;
            this.productService = productService;
        }

        public ExcelFileViewModel Handle(ConsolidationByUsernameExcelQuery message)
        {
            var param = CreateParameter(message);
            var reportData = reportService.GetConsolidationByUsername(param);
            var currencies = reportService
                .GetCurrencies(new GetCurrencyParam(message.ViewerId))
                .ToDictionary(c => c.Id, c => c.Name);
            var viewModels = BuildWinLossByUsernameViewModel(reportData, currencies);

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo("ConsolidationByUsername"))
                .Title(GetReporTitle(message, Resources.ConsolidationReportByUsername, currencies))
                .Header(Resources.ResourceManager.GetString)
                .Body(viewModels, GetRowMapper())
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), fileContent);
        }

        public ExcelFileViewModel Handle(ConsolidationByDateExcelQuery message)
        {
            var param = CreateParameter(message);
            var reportData = reportService.GetConsolidationByDate(param);
            var currencies = reportService
                .GetCurrencies(new GetCurrencyParam(message.ViewerId))
                .ToDictionary(c => c.Id, c => c.Name);
            var viewModels = BuildWinLossByDateViewModel(reportData, currencies);

            var fileContent = consolidationByDateWorkbook
                     .UseTemplate(GetTemplateInfo("ConsolidationByDate"))
                     .Title(GetReporTitle(message, Resources.ConsolidationReportByDate, currencies))
                     .Header(Resources.ResourceManager.GetString)
                     .Body(viewModels, null)
                     .Sheet(Resources.DefaultSheetName)
                     .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), fileContent);
        }

        public ExcelFileViewModel Handle(ConsolidationByProductExcelQuery message)
        {
            var param = CreateParameter(message);
            var reportData = reportService.GetConsolidationByProduct(param);
            var currencies = reportService
                .GetCurrencies(new GetCurrencyParam(message.ViewerId))
                .ToDictionary(c => c.Id, c => c.Name);
            var viewModels = BuildWinLossByProductViewModel(reportData, currencies);

            var fileContent = consolidationByProductWorkbook
                     .UseTemplate(GetTemplateInfo("ConsolidationByProduct"))
                     .Title(GetReporTitle(message, Resources.ConsolidationReportByProduct, currencies))
                     .Header(Resources.ResourceManager.GetString)
                     .Body(viewModels, null)
                     .Sheet(Resources.DefaultSheetName)
                     .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), fileContent);
        }

        public PagedReportData<WinLossDetailModel> Handle(ConsolidationDetailExcelQuery message)
        {
            var winLossDetailQuery = CreateWinLossDetailQuery(message);
            var param = CreateWinLossDetailParam(winLossDetailQuery);

            var reportData = winLossDetailService.GetWinLossDetail(param);

            return reportData;
        }

        private TemplateInfo GetTemplateInfo(string templateName)
            => new TemplateInfo(
                GetExcelTemplatePath(templateName),
                noInfomationText: Resources.NoInformationIsAvailable)
            .FreezePanel(4, C);

        private string GetReporTitle(GetConsolidationQuery message, string reportType, IDictionary<byte, string> currencies)
        {
            var currencyName = message.UserSelectedCurrencyId == DefaultVariables.AllCurrencies
                ? Resources.AllCurrencies
                : Resources.Currency + ": " + currencies[(byte)message.UserSelectedCurrencyId];

            return string.Join(
                " ",
                reportType,
                message.FromDate.ToShortDateWithSlash(),
                "-->",
                message.ToDate.ToShortDateWithSlash(),
                Resources.of2,
                productService.ConcatProductNames(message.UserSelectedProductIds),
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
                .Map(E, winLoss => winLoss.MemberWinLoss)
                .Map(F, winLoss => winLoss.MemberComm)
                .Map(G, winLoss => winLoss.MemberTotal)
                .Map(H, winLoss => winLoss.OwnerTurnover)
                .Map(I, winLoss => winLoss.OwnerWinLoss)
                .Map(J, winLoss => winLoss.OwnerComm)
                .Map(K, winLoss => winLoss.OwnerTotal)
                .Map(L, winLoss => winLoss.Company)
                .Map(M, winLoss => winLoss.BetCount);

            return mapper;
        }

        private string GetExcelFileName(GetConsolidationParam param)
            => string.Join(
                "-",
                Resources.Consolidation,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());
    }
}