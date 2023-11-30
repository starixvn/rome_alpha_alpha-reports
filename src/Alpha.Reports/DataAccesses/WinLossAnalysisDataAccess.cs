namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Criteria.WinLossAnalysis;
    using Fanex.Data.Repository;
    using Models;
    using Models.WinLossAnalysis;
    using Services;

    public interface WinLossAnalysisDataAccess
    {
        IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisCriteria criteria);
    }

    public class WinLossAnalysisDataAccessImpl : WinLossAnalysisDataAccess
    {
        private readonly IDynamicRepository repository;
        private readonly CacheRefService cacheRefService;

        public WinLossAnalysisDataAccessImpl(
            IDynamicRepository repository,
            CacheRefService cacheRefService)
        {
            this.repository = repository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossAnalysisModel> GetWinLossAnalysis(WinLossAnalysisCriteria criteria)
        {
            var isMasterOrSuperLevel = criteria.ReportLevel.IsMasterOrSuperLevel();
            var isSuperLevel = criteria.ReportLevel.IsSuperLevel();
            var isBySport = criteria.ReportType.Equals(WinLossAnalysisConstants.Sport);

            var data = repository.Fetch<WinLossAnalysisEntity>(criteria)
                .Select(wl => BuildWinLossAnalysisModel(isMasterOrSuperLevel, isSuperLevel, isBySport, wl));

            if (isBySport)
            {
                data = data.OrderBy(product => product.RollupKeyId);
            }

            return data.ToList();
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossAnalysisModel BuildWinLossAnalysisModel(
            bool isMasterOrSuperLevel,
            bool isSuperLevel,
            bool isBySport,
            WinLossAnalysisEntity entity)
        {
            int? rollupKeyId = null;

            if (isBySport)
            {
                rollupKeyId = int.Parse(entity.RollupKey);
                entity.RollupKey = cacheRefService.GetProductName(rollupKeyId.Value);
            }

            return new WinLossAnalysisModel(
                rollupKey: entity.RollupKey,
                rollupKeyId: rollupKeyId,
                turnover: entity.Turnover,
                betCount: entity.BetCount,
                memberNetWinLoss: new NetWinLoss(entity.MemberWinLoss, entity.MemberComm),
                agentNetWinLoss: new NetWinLoss(entity.AgWinLoss, entity.AgComm),
                masterNetWinLoss: isMasterOrSuperLevel ? new NetWinLoss(entity.MAgWinLoss.Value, entity.MAgComm.Value) : null,
                superNetWinLoss: isSuperLevel ? new NetWinLoss(entity.SAgWinLoss.Value, entity.SAgComm.Value) : null);
        }
    }
}