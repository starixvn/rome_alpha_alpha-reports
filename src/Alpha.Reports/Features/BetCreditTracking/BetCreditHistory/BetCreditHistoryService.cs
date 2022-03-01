namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;

    public interface BetCreditHistoryService
    {
        BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param);
    }

    public class BetCreditHistoryServiceImpl : BetCreditHistoryService
    {
        private readonly BetCreditHistoryDataAccess dataAccess;

        public BetCreditHistoryServiceImpl(BetCreditHistoryDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param)
        {
            var criteria = new BetCreditHistoryCriteria(param);

            var result = dataAccess.GetBetCreditHistory(criteria);

            return result;
        }
    }
}
