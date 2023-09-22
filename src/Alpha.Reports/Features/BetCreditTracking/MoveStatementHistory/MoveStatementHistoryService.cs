namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;

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
            var criteria = new MoveStatementHistoryCriteria(param);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var rawdata = await dynamicRepository.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo>(criteria);
            var reportData = Enumerable.Empty<MoveStatementHistoryByMember>();

            if (criteria.TotalRows > 0)
            {
                var memberRowHash = rawdata.Item2.ToDictionary(kvp => kvp.CustId, kvp => kvp);

                reportData = rawdata.Item1.GroupBy(
                    r => new { r.CustId, r.Username },
                    (key, g) => new MoveStatementHistoryByMember(
                            key.Username,
                            memberRowHash.TryGetValue(key.CustId, out var memberRow) ? memberRow : new MoveStatementHistoryByRefNo(),
                            g));
            }

            return new MoveStatementHistoryResult(criteria.ErrorCode, criteria.ErrorMess, reportData, criteria.TotalRows);
        }
    }
}
