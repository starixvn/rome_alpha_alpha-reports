namespace Alpha.Reports.Site.Handlers.SuperWinLossDetailByPT
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models.SuperWinLossDetailByPT;
    using Alpha.Reports.Params.SuperWinLossDetailByPT;
    using Alpha.Reports.Services;
    using App_GlobalResources;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests.SuperWinLossDetailByPT;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class SuperWinLossDetailByPTExcelHandler :
        SuperWinLossDetailByPTBaseHandler,
        IRequestHandler<SuperWinLossDetailByPTExcelQuery, ExcelFileViewModel>
    {
        private const string PTProductName = "PTProductName";
        private const int TitleStartIndex = 1;
        private const int HeaderStartIndex = 2;
        private const int BodyStartIndex = 4;
        private const int BodyEndIndex = 11;
        private const int FreezeRowIndex = 0;
        private const int SheetIndex = 1;
        private const int FooterEndIndex = 12;

        private readonly SuperWinLossDetailByPTWorkbook workbook;

        public SuperWinLossDetailByPTExcelHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            SuperWinLossDetailByPTService reportService,
            SuperWinLossDetailByPTWorkbook workbook)
            : base(productService, productDetailService, reportService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(SuperWinLossDetailByPTExcelQuery message)
        {
            var superPTParam = CreateSuperPTParam(message.ViewerId);
            var winLossDetailByPTParam = CreateSuperWinLossDetailByPTParam(message);

            var positionTakings = reportService.GetSuperPositionTakings(superPTParam);
            var winLossDetailByProducts = reportService.GetSuperWinLossDetailByProducts(winLossDetailByPTParam);

            var reportData = GenerateReportData(positionTakings, winLossDetailByProducts);
            var grandTotal = AccumulateGrandTotal(reportData);

            var fileContent = workbook
                .SetData(reportData, grandTotal)
                .SetRowMappers(GetBodyRowMapper(), GetTotalRowMapper())
                .UseTemplate(GetTemplateInfo(message))
                .Sheet(Resources.DefaultSheetName)
                .Title(GetReportTitle(winLossDetailByPTParam))
                .Header(key => key.Equals(PTProductName) ? $"{Resources.PT}/{Resources.Product}" : Resources.ResourceManager.GetString(key))
                .Footer(grandTotal, GetTotalRowMapper())
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossDetailByPTParam), fileContent);
        }

        private Mapper<SuperWinLossDetailModel> GetBodyRowMapper()
          => GetRowMapper(isTotal: false);

        private Mapper<SuperWinLossDetailModel> GetTotalRowMapper()
            => GetRowMapper(isTotal: true);

        internal Mapper<SuperWinLossDetailModel> GetRowMapper(bool isTotal)
        {
            var mapper = new Mapper<SuperWinLossDetailModel>()
                .Map(A, data => isTotal ? Resources.Total : data.ProductName)
                .Map(B, data => data.Turnover)
                .Map(C, data => data.NetTurnover)
                .Map(D, data => data.GrossComm)
                .Map(E, data => data.MemberWinLost)
                .Map(F, data => data.MemberComm)
                .Map(G, data => data.MemberTotal)
                .Map(H, data => data.AgWinLost)
                .Map(I, data => data.AgComm)
                .Map(J, data => data.AgTotal)
                .Map(K, data => data.MAgWinLost)
                .Map(L, data => data.MAgComm)
                .Map(M, data => data.MAgTotal)
                .Map(N, data => data.SAgWinLost)
                .Map(O, data => data.SAgComm)
                .Map(P, data => data.SAgTotal)
                .Map(Q, data => data.Company);

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(SuperWinLossDetailByPTExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: TitleStartIndex,
                headerStartIndex: HeaderStartIndex,
                bodyStartIndex: BodyStartIndex,
                bodyEndIndex: BodyEndIndex,
                freezeRowIndex: FreezeRowIndex,
                freezeColumnName: A,
                columnStartName: A,
                sheetIndex: SheetIndex,
                footerEndIndex: FooterEndIndex);

        private string GetReportTitle(SuperWinLossDetailByPTParam param)
        {
            var replacedProductNames = productDetailService
                .GetProductNamesExportExcel(param.UserSelectedResourceIds);
            var productNames = productService
                .ConcatProductNames(param.UserSelectedProductIds, replacedProductNames);

            return string.Join(
                " ",
                Resources.SuperWinLossDetailByPT,
                param.FromDate.ToShortDateWithSlash(),
                "-->",
                param.ToDate.ToShortDateWithSlash(),
                Resources.of2,
                productNames);
        }

        private string GetExcelFileName(
            SuperWinLossDetailByPTExcelQuery message,
            SuperWinLossDetailByPTParam param)
            => string.Join(
                "-",
                Resources.SuperWinLossDetailByPT,
                message.CustName,
                param.FromDate.ToShortDateWithSlash(),
                param.ToDate.ToShortDateWithSlash());
    }
}