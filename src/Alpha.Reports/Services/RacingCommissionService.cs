namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.RacingCommission;
    using Params;

    public interface RacingCommissionService
    {
        IEnumerable<RacingCommissionModel> GetCustomerCommission(RacingCommissionParam param);
    }

    public class RacingCommissionServiceImpl : RacingCommissionService
    {
        public IEnumerable<RacingCommissionModel> GetCustomerCommission(RacingCommissionParam param)
        {
            if (param.CustId <= 0 || param.CustLevel.IsUplineOf(param.UserLevel))
            {
                return Enumerable.Empty<RacingCommissionModel>();
            }

            return null;
        }
    }
}
