namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory
{
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;
    using System.Threading.Tasks;

    public interface MoveStatementHistoryService
    {
        Task<MoveStatementHistoryResult> GetMoveStatementHistory(MoveStatementHistoryParam param);
    }

    public class MoveStatementHistoryServiceImpl : MoveStatementHistoryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public MoveStatementHistoryServiceImpl(IDynamicRepository dynamicRepository, IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<MoveStatementHistoryResult> GetMoveStatementHistory(MoveStatementHistoryParam param)
        {
            var criteria = new MoveStatementHistoryCriteria(
                param.Username,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                param.ViewerId,
                param.PageIndex,
                param.PageSize,
                param.IsVariance);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var (moveStatementHistories, totalMoveStatementHistories, summaryMoveStatementHistories)
                = await dynamicRepository.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(criteria);
            var reportData = Enumerable.Empty<MoveStatementHistoryByMember>();
            var summaryData = new MoveStatementHistorySummary();

            if (criteria.TotalRows > 0)
            {
                var memberRowHash = totalMoveStatementHistories.ToDictionary(kvp => kvp.CustId, kvp => kvp);

                reportData = moveStatementHistories.GroupBy(
                    r => new { r.CustId, r.Username },
                    (key, g) => new MoveStatementHistoryByMember(
                            key.Username,
                            memberRowHash.TryGetValue(key.CustId, out var memberRow) ? memberRow : new MoveStatementHistoryByRefNo(),
                            g));
                summaryData = summaryMoveStatementHistories.FirstOrDefault();
            }

            return new MoveStatementHistoryResult(criteria.ErrorCode, criteria.ErrorMess, reportData, summaryData, criteria.TotalRows);
        }
    }
}