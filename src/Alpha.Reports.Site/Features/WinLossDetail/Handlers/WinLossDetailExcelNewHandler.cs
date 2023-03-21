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
            //var fileContent = workbook
            //   .UseTemplate(GetTemplateInfo(message))
            //   .Title(GetReportTitle(winLossDetailParam))
            //   .Header(Resources.ResourceManager.GetString)
            //   .Body(reportData.DataSource, GetBodyRowMapper(message))
            //   .Footer(reportData.TotalData, GetTotalRowMapper(message))
            //   .Sheet(Resources.DefaultSheetName)
            //   .ExportToBytes();

            workbook
               .UseTemplate(GetTemplateInfo(message))
               .Header(Resources.ResourceManager.GetString)
               .Sheet(Resources.DefaultSheetName);

            var winLossDetailParam = await CreateParameter(message, message.CustLevel);
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
                GetExcelTemplatePath(message.CustLevel.GetDirectDownLineLevel().DisplayName),
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