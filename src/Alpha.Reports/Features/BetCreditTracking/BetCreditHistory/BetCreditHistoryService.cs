namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface BetCreditHistoryService
    {
        Task<ReportData<BetCreditHistoryModel>> GetBetCreditHistoryAsync(BetCreditHistoryParam param);
    }

    public class BetCreditHistoryServiceImpl : BetCreditHistoryService
    {
        private readonly IDynamicRepository repository;

        public BetCreditHistoryServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public Task<ReportData<BetCreditHistoryModel>> GetBetCreditHistoryAsync(BetCreditHistoryParam param)
        {
            var list = new List<BetCreditHistoryModel>
            {
                new BetCreditHistoryModel { ActionDateTime = DateTime.Now,  WinLossDate = DateTime.Now, RefNo = 1, ActualStake = 100, WinLoss = 3, PlayerComm = 103, BalanceChange = 10, NewBalance = 20, GivenCredit = 10, BetCredit = 20, Action = "Test"  },
                new BetCreditHistoryModel { ActionDateTime = DateTime.Now,  WinLossDate = DateTime.Now, RefNo = 1, ActualStake = 100, WinLoss = 3, PlayerComm = 103, BalanceChange = 10, NewBalance = 20, GivenCredit = 10, BetCredit = 30, Action = "Test"  },
                new BetCreditHistoryModel { ActionDateTime = DateTime.Now,  WinLossDate = DateTime.Now, RefNo = 1, ActualStake = 100, WinLoss = 3, PlayerComm = 103, BalanceChange = 10, NewBalance = 20, GivenCredit = 10, BetCredit = 40, Action = "Test"  },
            };
            return Task.FromResult(new ReportData<BetCreditHistoryModel>(list, new BetCreditHistoryModel { ActionDateTime = DateTime.Now, WinLossDate = DateTime.Now, RefNo = 1, ActualStake = 100, WinLoss = 3, PlayerComm = 103, BalanceChange = 10, NewBalance = 20, GivenCredit = 10, BetCredit = 20, Action = "Test" }));
        }
    }
}
