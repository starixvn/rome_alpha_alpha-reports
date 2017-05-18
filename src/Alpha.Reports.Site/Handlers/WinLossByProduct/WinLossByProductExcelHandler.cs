namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Requests.WinLossByProduct;
    using Services;
    using Sunplus.Agent.Model.BL.Definitions;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossByProductExcelHandler :
        WinLossByProductBaseHandler,
        IRequestHandler<WinLossByProductExcelQuery, ExcelFileViewModel>
    {
        private readonly string betTypeName = nameof(WinLossByBetTypeModel.BetTypeName);
        private readonly WinLossByProductWorkbook workbook;

        public WinLossByProductExcelHandler(
            WinLossByProductService reportService,
            ProductService productService,
            CacheRefService cacheRefService,
            WinLossByProductWorkbook workbook) : base(reportService, productService, cacheRefService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(WinLossByProductExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossByBetTypeParam = CreateWinLossByBettypeParamater(message);
            var reportData = reportService.GetWinLossByProduct(winLossByBetTypeParam);
            var grandTotal = AccumulateGrandTotal(reportData);

            var fileContent = workbook
                .SetData(reportData, grandTotal)
                .SetRowMappers(
                    GetBodyRowMapper(message),
                    GetTotalRowMapper(message))
                .UseTemplate(GetTemplateInfo(message))
                .Sheet(Resources.DefaultSheetName)
                .Title(GetReporTitle(message))
                .Header(key => key.Equals(betTypeName) ? $"{Resources.Product}/{Resources.BetType}" : Resources.ResourceManager.GetString(key))
                .Footer(grandTotal, GetTotalRowMapper(message))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossByBetTypeParam), fileContent);
        }

        private Mapper<WinLossByBetTypeModel> GetBodyRowMapper(WinLossByProductExcelQuery message)
          => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossByBetTypeModel> GetTotalRowMapper(WinLossByProductExcelQuery message)
            => GetRowMapper(message, isTotal: true);

        private Mapper<WinLossByBetTypeModel> GetRowMapper(WinLossByProductExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossByBetTypeModel>()
                .Map(A, data => isTotal ? Resources.Total : BuildDisplayedBetTypeName(data.ProductId, (BetType)data.BetTypeId, data.BetTypeName, data.LiveIndicator))
                .Map(B, data => data.BetCount.HasValue ? (object)data.BetCount.Value : Resources.Symbol_NoDataNumber)
                .Map(C, data => data.Turnover)
                .Map(D, data => data.GrossComm);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(E, data => data.MemberNetWinLoss.WinLoss)
                 .Map(F, data => data.MemberNetWinLoss.Comm)
                 .Map(G, data => data.MemberNetWinLoss.Total)
                 .Map(H, data => data.AgentNetWinLoss.WinLoss)
                 .Map(I, data => data.AgentNetWinLoss.Comm)
                 .Map(J, data => data.AgentNetWinLoss.Total)
                 .Map(K, data => data.Company);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(E, data => data.AgentNetWinLoss.Total)
                   .Map(F, data => data.MasterNetWinLoss.WinLoss)
                   .Map(G, data => data.MasterNetWinLoss.Comm)
                   .Map(H, data => data.MasterNetWinLoss.Total)
                   .Map(I, data => data.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper.Map(E, data => data.MasterNetWinLoss.Total)
                    .Map(F, data => data.SuperNetWinLoss.WinLoss)
                    .Map(G, data => data.SuperNetWinLoss.Comm)
                    .Map(H, data => data.SuperNetWinLoss.Total)
                    .Map(I, data => data.Company);
            }

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(WinLossByProductExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 1,
                headerStartIndex: 2,
                bodyStartIndex: 4,
                bodyEndIndex: 11,
                freezeRowIndex: 0,
                freezeColumnName: A,
                columnStartName: A,
                sheetIndex: 1,
                footerEndIndex: 12);

        private string GetReporTitle(WinLossByProductExcelQuery message)
            => $"{Resources.WinLossByProduct} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}" +
            $" {Resources.of2} {productService.ConcatProductNames(message.UserSelectedProductIds)}";

        private string GetExcelFileName(WinLossByProductExcelQuery message, WinLossByProductParam winLossByBetTypeParam)
           => string.Join(
               "-",
               Resources.WinLossByProduct,
               message.CustName,
               winLossByBetTypeParam.FromDate.ToShortDateWithSlash(),
               winLossByBetTypeParam.ToDate.ToShortDateWithSlash());
    }
}