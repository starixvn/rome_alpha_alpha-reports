namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using App_GlobalResources;
    using Constants;
    using Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using RemotingRef;
    using Reports.Models;
    using Requests.MatchWinLossDetail;
    using Services;
    using ViewModels;
    using ViewModels.MatchWinLossDetail;
    using static ONEbook.SpreadSheets.Column;

    public class MatchWinLossDetailExcelHandler :
        MatchWinLossDetailBaseHandler,
        IRequestHandler<MatchWinLossDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly MatchWinLossDetailWorkbook workbook;

        public MatchWinLossDetailExcelHandler(
            ProductService productService,
            LeagueService leagueService,
            MatchWinLossDetailService reportService,
            CacheRefService cacheRefService,
            MatchWinLossDetailWorkbook workbook) :
            base(productService, leagueService, reportService, cacheRefService)
        {
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(MatchWinLossDetailExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var products = reportService.GetProductsForCustomer(message.CustId);
            var matchWinLossDetailParam = CreateParameter(message, products);
            var reportData = reportService.GetMatchWinLossDetail(matchWinLossDetailParam);

            var noParlayMatchWinLossDetail = GetNoParlayMatchWinLossDetail(reportData);
            var noParlayWinLossTotal = BuildWinLossTotal(noParlayMatchWinLossDetail);

            var mixParlay = GetMixParlay(reportData);
            var systemParlay = GetSystemParlay(reportData);
            var grandTotal = BuildWinLossGrandTotal(noParlayWinLossTotal, mixParlay, systemParlay);

            var fileContent = workbook
                .SetData(noParlayMatchWinLossDetail, noParlayWinLossTotal, mixParlay, systemParlay, grandTotal)
                .SetRowMappers(
                    GetBodyRowMapper(message.CustLevel),
                    GetSubTotalRowMapper(message.CustLevel),
                    GetTotalRowMapper(message.CustLevel),
                    GetGrandTotalRowMapper(message.CustLevel))
                .UseTemplate(GetTemplateInfo(message))
                .Sheet(Resources.DefaultSheetName)
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Footer(grandTotal, GetTotalRowMapper(message.CustLevel))
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        private TemplateInfo GetTemplateInfo(MatchWinLossDetailExcelQuery message)
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

        private string GetReportTitle(MatchWinLossDetailExcelQuery query)
            => $"{Resources.MatchWinLossDetail} {query.FromDate.ToShortDateWithSlash()} --> {query.ToDate.ToShortDateWithSlash()}" +
            $" {Resources.of2} {productService.ConcatProductNames(query.UserSelectedProductIds)}";

        private string GetExcelFileName(MatchWinLossDetailExcelQuery query)
            => string.Join(
                "-",
                Resources.MatchWinLossDetail,
                query.FromDate.ToShortDateWithSlash(),
                query.ToDate.ToShortDateWithSlash());

        private static Mapper<BetTypeWinLossViewModel> GetBodyRowMapper(UserLevel custLevel) => GetRowMapper(RowTypeConstant.Data, custLevel);

        private static Mapper<BetTypeWinLossViewModel> GetSubTotalRowMapper(UserLevel custLevel) => GetRowMapper(RowTypeConstant.SubTotal, custLevel);

        private static Mapper<BetTypeWinLossViewModel> GetTotalRowMapper(UserLevel custLevel) => GetRowMapper(RowTypeConstant.Total, custLevel);

        private static Mapper<BetTypeWinLossViewModel> GetGrandTotalRowMapper(UserLevel custLevel) => GetRowMapper(RowTypeConstant.GrandTotal, custLevel);

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        internal static Mapper<BetTypeWinLossViewModel> GetRowMapper(int rowType, UserLevel custLevel)
        {
            var mapper = new Mapper<BetTypeWinLossViewModel>();

            switch (rowType)
            {
                case RowTypeConstant.Data:
                    mapper.Map(A, betTypeWinLoss => betTypeWinLoss.Name);
                    break;

                case RowTypeConstant.SubTotal:
                    mapper.Map(A, betTypeWinLoss => Resources.SubTotal);
                    break;

                case RowTypeConstant.Total:
                    mapper.Map(A, betTypeWinLoss => Resources.Total);
                    break;

                case RowTypeConstant.GrandTotal:
                    mapper.Map(A, betTypeWinLoss => Resources.GrandTotal);
                    break;
            }

            mapper.Map(B, betTypeWinLoss => betTypeWinLoss.Turnover)
                .Map(C, betTypeWinLoss => betTypeWinLoss.GrossComm)
                .Map(D, betTypeWinLoss => betTypeWinLoss.MemberNetWinLoss.WinLoss)
                .Map(E, betTypeWinLoss => betTypeWinLoss.MemberNetWinLoss.Comm)
                .Map(F, betTypeWinLoss => betTypeWinLoss.MemberNetWinLoss.Total)
                .Map(G, betTypeWinLoss => betTypeWinLoss.AgentNetWinLoss.WinLoss)
                .Map(H, betTypeWinLoss => betTypeWinLoss.AgentNetWinLoss.Comm)
                .Map(I, betTypeWinLoss => betTypeWinLoss.AgentNetWinLoss.Total);

            if (custLevel.IsAgentLevel())
            {
                mapper.Map(J, betTypeWinLoss => betTypeWinLoss.CompanyNetWinLoss.Comm)
                    .Map(K, betTypeWinLoss => betTypeWinLoss.CompanyNetWinLoss.Total);
            }

            if (custLevel.IsMasterLevel())
            {
                mapper.Map(J, betTypeWinLoss => betTypeWinLoss.MasterNetWinLoss.WinLoss)
                    .Map(K, betTypeWinLoss => betTypeWinLoss.MasterNetWinLoss.Comm)
                    .Map(L, betTypeWinLoss => betTypeWinLoss.MasterNetWinLoss.Total)
                    .Map(M, betTypeWinLoss => betTypeWinLoss.CompanyNetWinLoss.Comm)
                    .Map(N, betTypeWinLoss => betTypeWinLoss.CompanyNetWinLoss.Total);
            }

            if (custLevel.IsSuperLevel())
            {
                mapper.Map(J, betTypeWinLoss => betTypeWinLoss.MasterNetWinLoss.WinLoss)
                    .Map(K, betTypeWinLoss => betTypeWinLoss.MasterNetWinLoss.Comm)
                    .Map(L, betTypeWinLoss => betTypeWinLoss.MasterNetWinLoss.Total)
                    .Map(M, betTypeWinLoss => betTypeWinLoss.SuperNetWinLoss.WinLoss)
                    .Map(N, betTypeWinLoss => betTypeWinLoss.SuperNetWinLoss.Comm)
                    .Map(O, betTypeWinLoss => betTypeWinLoss.SuperNetWinLoss.Total)
                    .Map(P, betTypeWinLoss => betTypeWinLoss.CompanyNetWinLoss.Comm)
                    .Map(Q, betTypeWinLoss => betTypeWinLoss.CompanyNetWinLoss.Total);
            }

            return mapper;
        }
    }
}