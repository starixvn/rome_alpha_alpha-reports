namespace Alpha.Reports.Features.WhiteLabelReport
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Reports.Features.WhiteLabelReport.Criteria;
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Features.WhiteLabelReport.Params;
    using Alpha.Reports.Models;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;

    public interface WhiteLabelReportService
    {
        Task<IEnumerable<IdNameModel>> GetWhiteLabels(int custId, string siteName);

        Task<ReportData<SettlementModel>> GetSettlement(ReportParam param);

        Task<ReportData<DailyWinLossModel>> GetDailyWinLoss(ReportParam param);
    }

    public class WhiteLabelReportServiceImpl : WhiteLabelReportService
    {
        private readonly IDynamicRepository repository;
        private readonly IPartialUMService partialUMService;

        public WhiteLabelReportServiceImpl(IDynamicRepository repository, IPartialUMService partialUMService)
        {
            this.repository = repository;
            this.partialUMService = partialUMService;
        }

        public Task<IEnumerable<IdNameModel>> GetWhiteLabels(int custId, string siteName)
        {
            var criteria = new GetWhiteLabelsCriteria(custId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

            return repository.FetchAsync<IdNameModel>(criteria);
        }

        public async Task<ReportData<SettlementModel>> GetSettlement(ReportParam param)
        {
            var criteria = new GetSettlementCriteria(
                param.ViewerId,
                param.FromDate,
                param.ToDate,
                param.WhiteLabels,
                param.Username);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            var settlement = await repository.FetchAsync<SettlementModel>(criteria);

            return new ReportData<SettlementModel>(settlement, AccumulateSettlementTotal(settlement));
        }

        private static SettlementModel AccumulateSettlementTotal(IEnumerable<SettlementModel> settlement)
        {
            var total = new SettlementModel();
            foreach (var item in settlement)
            {
                total += item;
            }

            return total;
        }

        public async Task<ReportData<DailyWinLossModel>> GetDailyWinLoss(ReportParam param)
        {
            var criteria = new GetDailyWinLossCriteria(param.ViewerId, param.FromDate, param.ToDate, param.WhiteLabels, param.Username);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            var winloss = await repository.FetchAsync<DailyWinLossModel>(criteria);

            return new ReportData<DailyWinLossModel>(winloss, AccumulateDailyWinLossTotal(winloss));
        }

        private static DailyWinLossModel AccumulateDailyWinLossTotal(IEnumerable<DailyWinLossModel> winloss)
        {
            var total = new DailyWinLossModel();
            foreach (var item in winloss)
            {
                total += item;
            }

            return total;
        }
    }
}
