namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
        private IDynamicRepository repository;

        public CommissionByBettypeServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param)
        {
            if (param.CustId <= 0 || param.CustLevel.IsUplineOf(param.UserLevel))
            {
                return Enumerable.Empty<CommissionByBettypeModel>();
            }

            var criteria = new CommissionByBettypeCriteria(param.CustLevel, param.CustId, param.FromDate, param.ToDate, param.MoveBetsDay);

            return repository.Fetch<CommissionByBettypeModel>(criteria);
        }
    }
}
