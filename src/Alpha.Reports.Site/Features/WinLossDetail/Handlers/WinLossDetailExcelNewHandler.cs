namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System.Threading.Tasks;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models.WinLossDetail;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossDetailExcelNewHandler :
        WinLossDetailBaseNewHandler,
        IAsyncRequestHandler<WinLossDetailExcelNewQuery, ExcelFileViewModel>
    {
        private readonly WinLossDetailService winlossDetailService;
        private readonly Workbook<WinLossDetailModel> workbook;

        public WinLossDetailExcelNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService winlossDetailService,
            Workbook<WinLossDetailModel> workbook) : base(productService, productDetailService)
        {
            this.winlossDetailService = winlossDetailService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossDetailExcelNewQuery message)
        {
            workbook
               .UseTemplate(GetTemplateInfo(message))
               .Header(Resources.ResourceManager.GetString)
               .Sheet(Resources.DefaultSheetName);

            var productsTask = productService.GetProductsForCustomerAsync(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(message.ViewerId, message.Site, message.CurrencyId);
            await Task.WhenAll(productsTask, productDetailsTask);
            var winLossDetailParam = CreateParameter(message, await productsTask, await productDetailsTask);
            var reportData = await winlossDetailService.GetWinLossDetailAsync(winLossDetailParam);

            var fileContent = workbook
                .Title(GetReportTitle(winLossDetailParam))
                .Body(reportData.DataSource, GetBodyRowMapper(message))
                .Footer(reportData.TotalData, GetTotalRowMapper(message))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, winLossDetailParam), fileContent);
        }

        private string GetReportTitle(WinLossDetailParam param)
        {
            var replacedProductNames = productDetailService.GetProductNamesExportExcel(param.UserSelectedResourceIds);
            var productNames = productService.ConcatProductNames(param.UserSelectedProductIds, replacedProductNames);

            return $"{Resources.WinLossDetail} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()} {Resources.of2} {productNames}";
        }

        private string GetExcelFileName(WinLossDetailExcelNewQuery message, WinLossDetailParam winLossDetailParam)
            => string.Join(
                "-",
                Resources.WinLossDetail,
                message.CustName,
                winLossDetailParam.FromDate.ToShortDateWithSlash(),
                winLossDetailParam.ToDate.ToShortDateWithSlash());

        private TemplateInfo GetTemplateInfo(WinLossDetailExcelNewQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath($"{message.CustLevel.GetDirectDownLineLevel().DisplayName}New"),
                noInfomationText: Resources.NoInformationIsAvailable)
            .FreezePanel(4, B);

        private Mapper<WinLossDetailModel> GetBodyRowMapper(WinLossDetailExcelNewQuery message)
            => GetRowMapper(message, isTotal: false);

        private Mapper<WinLossDetailModel> GetTotalRowMapper(WinLossDetailExcelNewQuery message)
            => GetRowMapper(message, isTotal: true);

        internal Mapper<WinLossDetailModel> GetRowMapper(WinLossDetailExcelNewQuery message, bool isTotal)
        {
            var mapper = new Mapper<WinLossDetailModel>()
                .Map(A, winLossDetail => isTotal ? Resources.Total : winLossDetail.CustName)
                .Map(B, winLossDetail => isTotal ? string.Empty : winLossDetail.FullName)
                .Map(C, winLossDetail => winLossDetail.BetCount)
                .Map(D, winLossDetail => winLossDetail.Turnover)
                .Map(E, winLossDetail => winLossDetail.NetTurnover)
                .Map(F, winLossDetail => winLossDetail.GrossComm)
                .Map(G, winLossDetail => winLossDetail.MemberNetWinLoss.WinLoss)
                .Map(H, winLossDetail => winLossDetail.MemberNetWinLoss.Comm)
                .Map(I, winLossDetail => winLossDetail.MemberNetWinLoss.Total)
                .Map(J, winLossDetail => winLossDetail.AgentNetWinLoss.WinLoss)
                .Map(K, winLossDetail => winLossDetail.AgentNetWinLoss.Comm)
                .Map(L, winLossDetail => winLossDetail.AgentNetWinLoss.Total);

            if (message.CustLevel.IsAgentLevel())
            {
                mapper.Map(M, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(M, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(N, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(O, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                mapper.Map(P, winLossDetail => winLossDetail.Company);
            }

            if (message.CustLevel.IsSuperLevel())
            {
                mapper
                    .Map(P, winLossDetail => winLossDetail.SuperNetWinLoss.WinLoss)
                    .Map(Q, winLossDetail => winLossDetail.SuperNetWinLoss.Comm)
                    .Map(R, winLossDetail => winLossDetail.SuperNetWinLoss.Total)
                    .Map(S, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }
    }
}