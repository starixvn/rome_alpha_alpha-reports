namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.RacingCommission;
    using Fanex.Data.Repository;
    using Models.RacingCommission;
    using Params;

    public interface RacingCommissionService
    {
        IEnumerable<RacingCommissionModel> GetCustomerCommission(RacingCommissionParam param);
    }

    public class RacingCommissionServiceImpl : RacingCommissionService
    {
        private IDynamicRepository repository;

        public RacingCommissionServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public IEnumerable<RacingCommissionModel> GetCustomerCommission(RacingCommissionParam param)
        {
            if (param.CustId <= 0 || param.CustLevel.IsUplineOf(param.UserLevel))
            {
                return Enumerable.Empty<RacingCommissionModel>();
            }

            var criteria = new RacingCommissionCriteria(param.CustLevel, param.CustId, param.FromDate, param.ToDate);

            return repository.Fetch<RacingCommissionModel>(criteria);
        }
    }
}
