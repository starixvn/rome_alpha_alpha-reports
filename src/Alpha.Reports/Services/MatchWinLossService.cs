namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.MatchWinLoss;
    using Fanex.Data.Repository;
    using Models;
    using Models.MatchWinLoss;
    using Params;

    public interface MatchWinLossService
    {
        IEnumerable<MatchWinLossModel> GetMatchWinLoss(MatchWinLossParam matchWinLossParam);
    }

    public class MatchWinLossServiceImpl : MatchWinLossService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public MatchWinLossServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        internal static DateRangeType DateRangeType { get; set; }

        public IEnumerable<MatchWinLossModel> GetMatchWinLoss(MatchWinLossParam matchWinLossParam)
        {
            if (matchWinLossParam == null)
            {
                throw new ArgumentNullException(nameof(matchWinLossParam));
            }

            if (!reportDateService.ValidateDateRange(
               new DateRange(DateRangeType, matchWinLossParam.FromDate, matchWinLossParam.ToDate, false)))
            {
                return Enumerable.Empty<MatchWinLossModel>();
            }

            var rawData = dynamicRepository.Fetch<MatchWinLossModel>(new GetMatchWinLossCriteria(
                matchWinLossParam.CustId,
                matchWinLossParam.FromDate,
                matchWinLossParam.ToDate));

            return rawData;
        }
    }
}
