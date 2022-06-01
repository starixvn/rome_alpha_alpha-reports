namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public interface MoveStatementHistoryService
    {
        MoveStatementHistoryResultModel GetMoveStatementHistory(MoveStatementHistoryParam param);
    }

    public class MoveStatementHistoryServiceImpl : MoveStatementHistoryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;


        public MoveStatementHistoryServiceImpl(IDynamicRepository dynamicRepository, PartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public MoveStatementHistoryResultModel GetMoveStatementHistory(MoveStatementHistoryParam param)
        {
            var criteria = new MoveStatementHistoryCriteria(param);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var result = dynamicRepository.Fetch<MoveStatementHistoryByRefNoModel>(criteria);

            IEnumerable<MoveStatementHistoryByMemberModel> reportData = result.GroupBy(
                r => new { r.CustId, r.Username },
                (key, g) => new MoveStatementHistoryByMemberModel(key.CustId, key.Username, g));

            return new MoveStatementHistoryResultModel(criteria.ErrorCode, criteria.ErrorMess, reportData, criteria.TotalRows);
        }
    }
}
