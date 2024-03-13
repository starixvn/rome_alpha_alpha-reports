namespace Alpha.Reports.Site.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Reports.Models;
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
        private readonly Workbook<WinLossDetailModel> workbook;

        public WinLossDetailExcelNewHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            WinLossDetailService winlossDetailService,
            CustomerService customerService,
            Workbook<WinLossDetailModel> workbook) : base(productService, productDetailService, winlossDetailService, customerService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossDetailExcelNewQuery message)
        {
            var isValidUsername = BuildWinLossDetailGetDataQuery(message);
            var isShowFullname = !string.IsNullOrWhiteSpace(message.ShowColumns) &&
                message.ShowColumns.IndexOf("fullname", StringComparison.OrdinalIgnoreCase) > -1;

            workbook
               .UseTemplate(GetTemplateInfo(message.CustLevel, isShowFullname))
               .Header(Resources.ResourceManager.GetString)
               .Sheet(Resources.DefaultSheetName);

            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var winLossDetailParam = CreateParameter(message, products, productDetails);
            var reportData = isValidUsername
                ? await winlossDetailService.GetWinLossDetailAsync(winLossDetailParam)
                : new PagedReportData<WinLossDetailModel>(null, message.PageIndex, message.PageSize, 0, null);

            var rowMapper = isShowFullname
                ? GetRowMapperWithFullname(message.CustLevel, false)
                : GetRowMapper(message.CustLevel, false);
            var totalMapper = isShowFullname
                ? GetRowMapperWithFullname(message.CustLevel, true)
                : GetRowMapper(message.CustLevel, true);
            var fileContent = workbook
                .Title(GetReportTitle(winLossDetailParam, productDetails))
                .Body(reportData.DataSource, rowMapper)
                .Footer(reportData.TotalData, totalMapper)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        private string GetReportTitle(WinLossDetailParam param, IEnumerable<ProductDetail> productDetails)
        {
            var replacedProductNames = productDetailService.GetProductNamesExportExcel(param.UserSelectedResourceIds, productDetails);
            var productNames = productService.ConcatProductNames(param.UserSelectedProductIds, replacedProductNames);

            return $"{Resources.WinLossDetail} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()} {Resources.of2} {productNames}";
        }

        private string GetExcelFileName(WinLossDetailExcelNewQuery message)
            => string.Join(
                "-",
                Resources.WinLossDetail,
                message.CustName,
                message.FromDate.ToShortDateWithSlash(),
                message.ToDate.ToShortDateWithSlash());

        private TemplateInfo GetTemplateInfo(UserLevel custLevel, bool isShowFullname)
        {
            var templatePath = GetExcelTemplatePath(
                custLevel.GetDirectDownLineLevel().DisplayName + (isShowFullname ? "New" : string.Empty));

            return new TemplateInfo(
                templatePath,
                noInfomationText: Resources.NoInformationIsAvailable)
            .FreezePanel(4, B);
        }

        internal Mapper<WinLossDetailModel> GetRowMapper(UserLevel custLevel, bool isTotal)
        {
            var mapper = new Mapper<WinLossDetailModel>()
                .Map(A, winLossDetail => isTotal ? Resources.Total : (winLossDetail.CustId == 0 ? Resources.DormantAccounts : winLossDetail.CustName))
                //.Map(B, winLossDetail => isTotal ? string.Empty : winLossDetail.FullName)
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

            if (custLevel.IsAgentLevel())
            {
                mapper.Map(L, winLossDetail => winLossDetail.Company);
            }

            if (custLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(L, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(M, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(N, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (custLevel.IsMasterLevel())
            {
                mapper.Map(O, winLossDetail => winLossDetail.Company);
            }

            if (custLevel.IsSuperLevel())
            {
                mapper
                    .Map(O, winLossDetail => winLossDetail.SuperNetWinLoss.WinLoss)
                    .Map(P, winLossDetail => winLossDetail.SuperNetWinLoss.Comm)
                    .Map(Q, winLossDetail => winLossDetail.SuperNetWinLoss.Total)
                    .Map(R, winLossDetail => winLossDetail.Company);
            }

            return mapper;
        }

        internal Mapper<WinLossDetailModel> GetRowMapperWithFullname(UserLevel custLevel, bool isTotal)
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

            if (custLevel.IsAgentLevel())
            {
                mapper.Map(M, winLossDetail => winLossDetail.Company);
            }

            if (custLevel.IsMasterOrSuperLevel())
            {
                mapper
                    .Map(M, winLossDetail => winLossDetail.MasterNetWinLoss.WinLoss)
                    .Map(N, winLossDetail => winLossDetail.MasterNetWinLoss.Comm)
                    .Map(O, winLossDetail => winLossDetail.MasterNetWinLoss.Total);
            }

            if (custLevel.IsMasterLevel())
            {
                mapper.Map(P, winLossDetail => winLossDetail.Company);
            }

            if (custLevel.IsSuperLevel())
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