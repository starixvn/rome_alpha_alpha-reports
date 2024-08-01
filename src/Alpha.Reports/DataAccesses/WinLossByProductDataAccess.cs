namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Criteria.WinLossByProductCriteria;
    using Fanex.Data.Repository;
    using Models;
    using Models.WinlossByBetType;
    using Services;

    public interface WinLossByProductDataAccess
    {
        IEnumerable<WinLossByBetTypeModel> GetWinLossByBetType(WinLossByProductCriteria criteria);
    }

    public class WinLossByProductDataAccessImpl : WinLossByProductDataAccess
    {
        private const string mMROddTypeId = "6";

        private readonly IDynamicRepository repository;
        private readonly CacheRefService cacheRefService;

        public WinLossByProductDataAccessImpl(
            IDynamicRepository repository,
            CacheRefService cacheRefService)
        {
            this.repository = repository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossByBetTypeModel> GetWinLossByBetType(WinLossByProductCriteria criteria)
        {
            var rawdata = repository.Fetch<WinLossByBetTypeEntity>(criteria);

            if (rawdata.Any())
            {
                var mMROddTypeName = cacheRefService.GetOddsTypeName(mMROddTypeId);

                return rawdata
                    .Select(wl => BuildWinLossByBetType(wl, criteria.CustLevel, mMROddTypeName))
                    .ToList();
            }

            return Enumerable.Empty<WinLossByBetTypeModel>();
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private WinLossByBetTypeModel BuildWinLossByBetType(WinLossByBetTypeEntity entity, UserLevel reportLevel, string mMROddTypeName)
        {
            var sportTypeId = entity.SportType ?? 0;

            var winLossByBettype = new WinLossByBetTypeModel(
                 betTypeId: entity.BetType,
                 betTypeName: BuildBetTypeName(cacheRefService.GetBetTypeName(entity.BetType), mMROddTypeName, entity.IsMMR),
                 productId: entity.ProductId ?? 0,
                 sportTypeId: sportTypeId,
                 sportTypeName: sportTypeId == 0 ? string.Empty : cacheRefService.GetSportName(sportTypeId),
                 liveIndicator: entity.LiveIndicator,
                 betCount: entity.BetCount,
                 turnover: entity.Turnover,
                 grossComm: entity.GrossComm,
                 memberNetWinLoss: reportLevel.IsAgentLevel() ? new NetWinLoss(entity.MemberWinLoss, entity.MemberComm) : null,
                 agentNetWinLoss: ReadAgentNetWinLoss(entity, reportLevel),
                 masterNetWinLoss: ReadMasterNetWinLoss(entity, reportLevel),
                 superNetWinLoss: ReadSuperNetWinLoss(entity, reportLevel));

            return winLossByBettype;
        }

        private static string BuildBetTypeName(string betTypeName, string mMROddTypeName, bool isMMR)
            => isMMR ? string.Format("{0} ({1})", betTypeName, mMROddTypeName) : betTypeName;

        private static NetWinLoss ReadAgentNetWinLoss(WinLossByBetTypeEntity entity, UserLevel reportLevel)
            => reportLevel.IsNotSuperLevel()
            ? (reportLevel.IsMasterLevel()
                ? new NetWinLoss(entity.DownlineTotal)
                : new NetWinLoss(entity.WinLoss, entity.Comm))
            : null;

        private static NetWinLoss ReadMasterNetWinLoss(WinLossByBetTypeEntity entity, UserLevel reportLevel)
            => reportLevel.IsMasterOrSuperLevel()
            ? (reportLevel.IsMasterLevel()
                ? new NetWinLoss(entity.WinLoss, entity.Comm)
                : new NetWinLoss(entity.DownlineTotal))
            : null;

        private static NetWinLoss ReadSuperNetWinLoss(WinLossByBetTypeEntity entity, UserLevel reportLevel)
            => reportLevel.IsSuperLevel()
            ? new NetWinLoss(entity.WinLoss, entity.Comm)
            : null;
    }
}