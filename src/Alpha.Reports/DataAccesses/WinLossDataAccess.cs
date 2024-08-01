namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLoss;
    using Fanex.Data.Repository;
    using Models;
    using Models.Winloss;

    public interface WinLossDataAccess
    {
        IEnumerable<WinLossModel> GetWinLoss(WinLossCriteria criteria);
    }

    public class WinLossDataAccessImpl : WinLossDataAccess
    {
        private readonly IDynamicRepository repository;

        public WinLossDataAccessImpl(IDynamicRepository repository)
            => this.repository = repository;

        public IEnumerable<WinLossModel> GetWinLoss(WinLossCriteria criteria)
            => repository.Fetch<WinLossEntity>(criteria)
            .Select(wl => BuildWinLoss(wl, criteria.ReportLevel))
            .ToList();
        
        private static WinLossModel BuildWinLoss(WinLossEntity entity, UserLevel reportLevel)
            => new WinLossModel(
            custId: entity.CustId,
            custName: entity.CustName,
            turnover: entity.Turnover,
            grossComm: entity.GrossComm,
            memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(entity.MemberWinLoss, entity.MemberComm) : null,
            agentNetWinLoss: ReadAgentNetWinLoss(entity, reportLevel),
            masterNetWinLoss: ReadMasterNetWinLoss(entity, reportLevel),
            superNetWinLoss: ReadSuperNetWinLoss(entity, reportLevel));

        private static NetWinLoss ReadAgentNetWinLoss(WinLossEntity entity, UserLevel reportLevel)
            => reportLevel.IsNotSuperLevel()
            ? (reportLevel.IsMasterLevel()
                ? new NetWinLoss(entity.DownlineTotal)
                : new NetWinLoss(entity.WinLoss, entity.Comm))
            : null;

        private static NetWinLoss ReadMasterNetWinLoss(WinLossEntity entity, UserLevel reportLevel)
            => reportLevel.IsMasterOrSuperLevel()
            ? (reportLevel.IsMasterLevel()
                ? new NetWinLoss(entity.WinLoss, entity.Comm)
                : new NetWinLoss(entity.DownlineTotal))
            : null;

        private static NetWinLoss ReadSuperNetWinLoss(WinLossEntity entity, UserLevel reportLevel)
            => reportLevel.IsSuperLevel()
            ? new NetWinLoss(entity.WinLoss, entity.Comm)
            : null;
    }
}