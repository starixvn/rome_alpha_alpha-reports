namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.MatchWinLoss;
    using Fanex.Data.Repository;
    using Models.MatchWinLoss;
    using Params;

    public interface MatchWinLossService
    {
        IEnumerable<MatchWinLossModel> GetMatchWinLoss(MatchWinLossParam matchWinLossParam);
    }

    public class MatchWinLossServiceImpl : MatchWinLossService
    {
        private readonly IDynamicRepository dynamicRepository;

        public MatchWinLossServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<MatchWinLossModel> GetMatchWinLoss(MatchWinLossParam matchWinLossParam)
        {
            if (matchWinLossParam == null)
            {
                throw new ArgumentNullException(nameof(matchWinLossParam));
            }

            var rawData = dynamicRepository.Fetch<MatchWinLossModel>(new GetMatchWinLossCriteria(
                matchWinLossParam.CustId,
                matchWinLossParam.FromDate,
                matchWinLossParam.ToDate));

            return rawData;
        }
    }
}
