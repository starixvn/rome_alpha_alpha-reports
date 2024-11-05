namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.WinLossDetail;
    using Fanex.Data.Repository;
    using Models;
    using Models.WinLossDetail;

    public interface WinLossDetailDataAccess
    {
        PagedReportData<WinLossDetailModel> GetData(WinLossDetailMemberCriteria criteria);

        Task<PagedReportData<WinLossDetailModel>> GetDataAsync(WinLossDetailMemberCriteria criteria);
    }

    public class WinLossDetailDataAccessImpl : WinLossDetailDataAccess
    {
        private readonly IDynamicRepository repository;

        public WinLossDetailDataAccessImpl(IDynamicRepository repository)
            => this.repository = repository;

        public PagedReportData<WinLossDetailModel> GetData(WinLossDetailMemberCriteria criteria)
        {
            var (winLossEntities, totalWinLossEntities) = repository.FetchMultiple<WinLossDetailEntity, WinLossDetailEntity>(criteria);

            return BuildWinLossDetailReportData(winLossEntities, totalWinLossEntities, criteria);
        }

        public async Task<PagedReportData<WinLossDetailModel>> GetDataAsync(WinLossDetailMemberCriteria criteria)
        {
            var (winLossEntities, totalWinLossEntities) = await repository.FetchMultipleAsync<WinLossDetailEntity, WinLossDetailEntity>(criteria);

            return BuildWinLossDetailReportData(winLossEntities, totalWinLossEntities, criteria);
        }

        private static PagedReportData<WinLossDetailModel> BuildWinLossDetailReportData(
            IEnumerable<WinLossDetailEntity> winLossEntities,
            IEnumerable<WinLossDetailEntity> totalWinLossEntities,
            WinLossDetailMemberCriteria criteria)
        {
            var winLossDetails = winLossEntities.Select(wl => BuildWinLossDetail(wl, criteria.ReportLevel));
            var totalWinLoss = totalWinLossEntities.Select(wl => BuildWinLossDetail(wl, criteria.ReportLevel));

            return new PagedReportData<WinLossDetailModel>(
                winLossDetails.ToList(),
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRecords,
                totalWinLoss.FirstOrDefault());
        }

        private static WinLossDetailModel BuildWinLossDetail(WinLossDetailEntity entity, UserLevel reportLevel)
            => new WinLossDetailModel(
            custId: entity.CustId ?? 0,
            custName: entity.CustName,
            firstName: entity.FirstName,
            lastName: entity.LastName,
            isExistedWatchlist: reportLevel.IsAgentLevel() && entity.IsExistedWatchList == true,
            turnover: entity.Turnover,
            netTurnover: entity.NetTurnover,
            grossComm: entity.GrossComm,
            betCount: entity.NumberOfTickets,
            memberNetWinLoss: new NetWinLoss(entity.MemberWinLoss, entity.MemberComm),
            agentNetWinLoss: new NetWinLoss(entity.AgWinLoss, entity.AgComm),
            masterNetWinLoss: reportLevel.IsMasterOrSuperLevel() ? new NetWinLoss(entity.MAgWinLoss ?? 0, entity.MAgComm ?? 0) : null,
            superNetWinLoss: reportLevel.IsSuperLevel() ? new NetWinLoss(entity.SAgWinLoss ?? 0, entity.SAgComm ?? 0) : null,
            rewardAmount: entity.RewardAmount,
            rewardCurrency: entity.RewardCurrency,
            isDirectMember: entity.IsDirectMember);
    }
}