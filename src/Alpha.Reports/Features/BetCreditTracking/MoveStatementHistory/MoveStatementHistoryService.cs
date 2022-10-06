namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.PartialUM.Services;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;

    public interface MoveStatementHistoryService
    {
        MoveStatementHistoryResultModel GetMoveStatementHistory(MoveStatementHistoryParam param);
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

        public MoveStatementHistoryResultModel GetMoveStatementHistory(MoveStatementHistoryParam param)
        {
            var criteria = new MoveStatementHistoryCriteria(param);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var result = dynamicRepository.Fetch<MoveStatementHistoryByRefNoModel>(criteria);

            IEnumerable<MoveStatementHistoryByMemberModel> reportData = result.GroupBy(
                r => new { r.CustId, r.Username },
                (key, g) => new MoveStatementHistoryByMemberModel(key.CustId, key.Username, g));

            return new MoveStatementHistoryResultModel(criteria.ErrorCode, criteria.ErrorMess, reportData, criteria.TotalRows);
        }
    }
}
