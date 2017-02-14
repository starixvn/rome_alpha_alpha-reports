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

            IList<CommissionByBettypeModel> commissionByBettypes = new List<CommissionByBettypeModel>()
            {
                new CommissionByBettypeModel { CustId = 1, CustName = "UserName1", TotalComm1= 11, TotalComm2 = 21, TotalComm3 = 31, ChargeComm = 1, MOComm = 2, CasinoComm = 3, OtherComm = 4},
                new CommissionByBettypeModel { CustId = 2, CustName = "UserName2", TotalComm1= 12, TotalComm2 = 22, TotalComm3 = 32, ChargeComm = 1, MOComm = 2, CasinoComm = 3, OtherComm = 4},
                new CommissionByBettypeModel { CustId = 3, CustName = "UserName3", TotalComm1= 13, TotalComm2 = 23, TotalComm3 = 33, ChargeComm = 1, MOComm = 2, CasinoComm = 3, OtherComm = 4},
                new CommissionByBettypeModel { CustId = 4, CustName = "UserName4", TotalComm1= 14, TotalComm2 = 24, TotalComm3 = 34, ChargeComm = 1, MOComm = 2, CasinoComm = 3, OtherComm = 4},
                new CommissionByBettypeModel { CustId = 5, CustName = "UserName5", TotalComm1= 15, TotalComm2 = 25, TotalComm3 = 35, ChargeComm = 1, MOComm = 2, CasinoComm = 3, OtherComm = 4},
            };

            return commissionByBettypes;
            //return repository.Fetch<CommissionByBettypeModel>(criteria);
        }
    }
}
