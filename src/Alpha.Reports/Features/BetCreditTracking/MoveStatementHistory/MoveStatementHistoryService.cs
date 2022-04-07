namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public interface MoveStatementHistoryService
    {
        MoveStatementHistoryResultModel GetMoveStatementHistory(MoveStatementHistoryParam param);
    }

    public class MoveStatementHistoryServiceImpl : MoveStatementHistoryService
    {
        private readonly IDynamicRepository repository;
        
        public MoveStatementHistoryServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public MoveStatementHistoryResultModel GetMoveStatementHistory(MoveStatementHistoryParam param)
        {
            var criteria = new MoveStatementHistoryCriteria(param);

            var result = repository.Fetch<MoveStatementHistoryByRefNoModel>(criteria);

            IEnumerable<MoveStatementHistoryByMemberModel> reportData = result.GroupBy(
                r => new { r.CustId, r.Username },
                (key, g) => new MoveStatementHistoryByMemberModel(key.CustId, key.Username, g));

            return new MoveStatementHistoryResultModel(criteria.ErrorCode, criteria.ErrorMess, reportData);
        }
    }
}
