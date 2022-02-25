namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface MoveStatementHistoryService
    {
        Task<ReportData<MoveStatementHistoryModel>> GetMoveStatementHistoryAsync(MoveStatementHistoryParam param);
    }

    public class MoveStatementHistoryServiceImpl : MoveStatementHistoryService
    {
        private readonly IDynamicRepository repository;
        
        public MoveStatementHistoryServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public Task<ReportData<MoveStatementHistoryModel>> GetMoveStatementHistoryAsync(MoveStatementHistoryParam param)
        {
            var list = new List<MoveStatementHistoryModel>
            {
                new MoveStatementHistoryModel { MemberRefNo = "test",  OldEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 100, MemberComm = 3, MemberTotal = 103  },
                new MoveStatementHistoryModel { MemberRefNo = "test",  OldEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 100, MemberComm = 3, MemberTotal = 103  },
                new MoveStatementHistoryModel { MemberRefNo = "test",  OldEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 100, MemberComm = 3, MemberTotal = 103  },
            };
            return Task.FromResult(new ReportData<MoveStatementHistoryModel>(list, new MoveStatementHistoryModel { MemberRefNo = "total", OldEventDate = DateTime.Now, NewEventDate = DateTime.Now, MemberWinLoss = 300, MemberComm = 9, MemberTotal = 309 }));
        }
    }
}
