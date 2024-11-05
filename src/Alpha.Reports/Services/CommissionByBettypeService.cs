namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Services;
    using Criteria.CommissionByBettype;
    using Fanex.Data.Repository;
    using Models.CommissionByBettype;
    using Params;

    public interface CommissionByBettypeService
    {
        IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param);
    }

    public class CommissionByBettypeServiceImpl : CommissionByBettypeService
    {
        private readonly IDynamicRepository repository;
        private readonly IPartialUMService partialUMService;

        public CommissionByBettypeServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            repository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param)
        {
            var criteria = GetCommissionByBettypeCriteria(param);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return repository.Fetch<CommissionByBettypeModel>(criteria);
        }

        private ICriteria GetCommissionByBettypeCriteria(CommissionByBettypeParam param)
        {
            if (param.CustLevel.IsAgentLevel())
            {
                return new CommissionByBettypeMemberCriteria(
                    param.CustId,
                    param.FromDate,
                    param.ToDate,
                    param.MoveBetsDay);
            }

            if (param.CustLevel.IsMasterLevel())
            {
                return new CommissionByBettypeAgentCriteria(
                    param.CustId,
                    param.FromDate,
                    param.ToDate,
                    param.MoveBetsDay,
                    param.LevelType);
            }

            return new CommissionByBettypeMasterCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                param.MoveBetsDay,
                param.LevelType);
        }
    }
}