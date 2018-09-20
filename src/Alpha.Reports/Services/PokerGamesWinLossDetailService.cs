namespace Alpha.Reports.Services
{
    using System;
    using Criteria.PokerGamesWinLossDetail;
    using DataAccesses;
    using Models;
    using Models.PokerGamesWinLossDetail;
    using Params;

    public interface PokerGamesWinLossDetailService
    {
        PagedReportData<PokerGamesWinLossDetailModel> GetWinLossDetail(PokerGamesWinLossDetailParam param);
    }

    public class PokerGamesWinLossDetailServiceImpl : PokerGamesWinLossDetailService
    {
        private readonly ReportDateService reportDateService;
        private readonly PokerGamesWinLossDetailDataAccess dataAccess;

        public PokerGamesWinLossDetailServiceImpl(
            ReportDateService reportDateService,
            PokerGamesWinLossDetailDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.dataAccess = dataAccess;
        }

        public PagedReportData<PokerGamesWinLossDetailModel> GetWinLossDetail(PokerGamesWinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new PokerGamesWinLossDetailCriteria(param, reportDateService.GetBalanceUpDay(), reportDateService.GetMoveBetsDay());

            var winLossDetailData = dataAccess.GetData(criteria);

            return winLossDetailData;
        }
    }
}