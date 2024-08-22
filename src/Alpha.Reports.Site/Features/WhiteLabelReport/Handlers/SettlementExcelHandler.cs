namespace Alpha.Reports.Site.Features.WhiteLabelReport
{
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.WhiteLabelReport;
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Features.WhiteLabelReport.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.WhiteLabelReport.Requests;
    using Alpha.Reports.Site.Handlers;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;

    public class SettlementExcelHandler :
        BaseHandler,
        IAsyncRequestHandler<SettlementExcelQuery, ExcelFileViewModel>
    {
        private readonly WhiteLabelReportService reportService;
        private readonly Workbook<SettlementModel> workbook;

        public SettlementExcelHandler(
            WhiteLabelReportService reportService,
            Workbook<SettlementModel> workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(SettlementExcelQuery message)
        {
            var reportData = await GetSettlement(message);

            var result = workbook
                .UseTemplate(GetTemplateInfo(message.ViewerLevel))
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .Body(reportData.DataSource, GetRowMapper(message, false))
                .Footer(reportData.TotalData, GetRowMapper(message, true))
                .Sheet(Resources.DefaultSheetName)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), result);
        }

        private Task<ReportData<SettlementModel>> GetSettlement(SettlementGetDataQuery message)
        {
            if (message.ViewerLevel.IsAgentLevel())
            {
                message.WhiteLabels = null;
                message.Username = null;
            }

            return reportService.GetSettlement(new ReportParam(
                message.ViewerId,
                message.FromDate.FirstDayOfMonth(),
                message.ToDate.LastDayOfMonth(),
                message.WhiteLabels,
                message.Username,
                message.Site));
        }

        private TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                GetExcelTemplatePath($"Settlement{viewerLevel.GetDirectDownLineLevel().DisplayName}"),
                noInfomationText: Resources.NoInformationIsAvailable);

        private static string GetReportTitle(SettlementExcelQuery message)
            => $"{Resources.SettlementReport} {message.FromDate.ToMMYYYYWithSlash()} --> {message.ToDate.ToMMYYYYWithSlash()}";

        internal static Mapper<SettlementModel> GetRowMapper(SettlementExcelQuery message, bool isTotal)
        {
            if (message.ViewerLevel.IsSuperLevel())
            {
                return GetSuperRowMapper(isTotal);
            }

            if (message.ViewerLevel.IsMasterLevel())
            {
                return GetMasterRowMapper(isTotal);
            }

            return GetAgentRowMapper(isTotal);
        }

        internal static Mapper<SettlementModel> GetSuperRowMapper(bool isTotal)
        {
            var mapper = new Mapper<SettlementModel>();
            if (isTotal)
            {
                mapper.Map(A, settlement => Resources.Total);
            }
            else
            {
                mapper
                    .Map(A, settlement => settlement.Month)
                    .Map(B, settlement => settlement.WhiteLabel)
                    .Map(C, settlement => settlement.Agent)
                    .Map(D, settlement => settlement.Master);
            }

            return mapper
                .Map(E, settlement => settlement.LossCarryforward)
                .Map(F, settlement => settlement.MemberWL)
                .Map(G, settlement => settlement.MemberPromotion)
                .Map(H, settlement => settlement.WhiteLabelNetWinLoss)
                .Map(I, settlement => settlement.WhiteLabelIncome)
                .Map(J, settlement => settlement.WhiteLabelPromotionShare)
                .Map(K, settlement => settlement.WhiteLabelFee)
                .Map(L, settlement => settlement.WhiteLabelPromotionSettlement)
                .Map(M, settlement => settlement.WhiteLabelTotal)
                .Map(N, settlement => settlement.MasterEarning)
                .Map(O, settlement => settlement.SuperEarning)
                .Map(P, settlement => settlement.CompanyIncome)
                .Map(Q, settlement => settlement.CompanyPromotionShare)
                .Map(R, settlement => settlement.CompanyTotal);
        }

        internal static Mapper<SettlementModel> GetMasterRowMapper(bool isTotal)
        {
            var mapper = new Mapper<SettlementModel>();
            if (isTotal)
            {
                mapper.Map(A, settlement => Resources.Total);
            }
            else
            {
                mapper
                    .Map(A, settlement => settlement.Month)
                    .Map(B, settlement => settlement.WhiteLabel)
                    .Map(C, settlement => settlement.Agent);
            }

            return mapper
                .Map(D, settlement => settlement.LossCarryforward)
                .Map(E, settlement => settlement.MemberWL)
                .Map(F, settlement => settlement.MemberPromotion)
                .Map(G, settlement => settlement.WhiteLabelNetWinLoss)
                .Map(H, settlement => settlement.WhiteLabelIncome)
                .Map(I, settlement => settlement.WhiteLabelPromotionShare)
                .Map(J, settlement => settlement.WhiteLabelFee)
                .Map(K, settlement => settlement.WhiteLabelPromotionSettlement)
                .Map(L, settlement => settlement.WhiteLabelTotal)
                .Map(M, settlement => settlement.MasterEarning)
                .Map(N, settlement => settlement.CompanyIncome)
                .Map(O, settlement => settlement.CompanyPromotionShare)
                .Map(P, settlement => settlement.CompanyTotal);
        }

        internal static Mapper<SettlementModel> GetAgentRowMapper(bool isTotal)
        {
            var mapper = new Mapper<SettlementModel>();
            if (isTotal)
            {
                mapper.Map(A, settlement => Resources.Total);
            }
            else
            {
                mapper .Map(A, settlement => settlement.Month);
            }

            return mapper
                .Map(B, settlement => settlement.LossCarryforward)
                .Map(C, settlement => settlement.MemberWL)
                .Map(D, settlement => settlement.MemberPromotion)
                .Map(E, settlement => settlement.WhiteLabelNetWinLoss)
                .Map(F, settlement => settlement.WhiteLabelIncome)
                .Map(G, settlement => settlement.WhiteLabelPromotionShare)
                .Map(H, settlement => settlement.WhiteLabelFee)
                .Map(I, settlement => settlement.WhiteLabelPromotionSettlement)
                .Map(J, settlement => settlement.WhiteLabelTotal)
                .Map(K, settlement => settlement.CompanyIncome)
                .Map(L, settlement => settlement.CompanyPromotionShare)
                .Map(M, settlement => settlement.CompanyTotal);
        }

        private string GetExcelFileName(SettlementExcelQuery message)
           => string.Join(
               "-",
               Resources.SettlementReport,
               message.CustName,
               message.FromDate.ToMMYYYYWithSlash(),
               message.ToDate.ToMMYYYYWithSlash());
    }
}