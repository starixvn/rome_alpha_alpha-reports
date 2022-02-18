namespace Alpha.Reports.Features.BetCreditTracking.StatementMovement
{
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Models;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Params;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface StatementMovementService
    {
        Task<ReportData<StatementMovementModel>> GetStatementMovementDetailAsync(StatementMovementParam param);
    }

    public class StatementMovementServiceImpl : StatementMovementService
    {
        private readonly IDynamicRepository repository;
        
        public StatementMovementServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public Task<ReportData<StatementMovementModel>> GetStatementMovementDetailAsync(StatementMovementParam param)
        {
            var list = new List<StatementMovementModel>
            {
                new StatementMovementModel { MemberRefNo = "test",  OddEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 100, MemberComm = 3, MemberTotal = 103  },
                new StatementMovementModel { MemberRefNo = "test",  OddEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 100, MemberComm = 3, MemberTotal = 103  },
                new StatementMovementModel { MemberRefNo = "test",  OddEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 100, MemberComm = 3, MemberTotal = 103  },
            };
            return Task.FromResult(new ReportData<StatementMovementModel>(list, new StatementMovementModel { MemberRefNo = "total", OddEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 300, MemberComm = 9, MemberTotal = 309 }));
        }
    }
}
