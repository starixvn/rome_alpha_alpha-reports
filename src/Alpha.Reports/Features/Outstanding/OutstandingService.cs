namespace Alpha.Reports.Services.Outstanding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Features.AutoFollowBets;
    using Alpha.Reports.Features.AutoFollowBets.Models;
    using AlphaPlus.PartialUM.Services;
    using Criteria.Outstanding;
    using Fanex.Data.Repository;
    using Models.Outstanding;
    using Params;

    public interface OutstandingService
    {
        IEnumerable<OutstandingModel> GetMemberOutstanding(OutstandingParam param);
    }

    public class OutstandingServiceImpl : OutstandingService
    {
        private readonly IDynamicRepository repository;
        private readonly IPartialUMService partialUMService;
        private readonly AutoFollowBetService autoFollowBetService;

        public OutstandingServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            AutoFollowBetService autoFollowBetService)
        {
            repository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.autoFollowBetService = autoFollowBetService;
        }

        public IEnumerable<OutstandingModel> GetMemberOutstanding(OutstandingParam param)
        {
            var criteria = GetOutstandingCriteria(param);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return param.AutoFollowBetIsDummyAgent
                ? GetMemberOutstanding(
                    param.ProductIds,
                    autoFollowBetService.GetFollowBetAccounts(param.CustId))
                : repository.Fetch<OutstandingModel>(criteria);
        }

        private IEnumerable<OutstandingModel> GetMemberOutstanding(
            string productIds,
            IEnumerable<Tuple<int, IEnumerable<FollowBetAccount>>> followBetAccounts)
        {
            var outstandingModels = new List<OutstandingModel>();

            foreach (var item in followBetAccounts)
            {
                outstandingModels.AddRange(GetMemberOutstanding(productIds, item));
            }

            return outstandingModels;
        }

        private IEnumerable<OutstandingModel> GetMemberOutstanding(
            string productIds,
            Tuple<int, IEnumerable<FollowBetAccount>> followBetAccount)
        {
            var agentId = followBetAccount.Item1;
            var followBetAccountEntitys = followBetAccount.Item2;
            var outstandingModels = repository.Fetch<OutstandingModel>(new OutstandingMemberCriteria(agentId, productIds));

            return outstandingModels.Join(
                followBetAccountEntitys,
                outstanding => outstanding.CustId, account => account.CustId,
                (outstanding, account) =>
                {
                    outstanding.CustId = account.DummyCustId;
                    outstanding.CustName = account.DummyUsername;
                    return outstanding;
                });
        }

        private static ICriteria GetOutstandingCriteria(OutstandingParam param)
        {
            if (param.CustLevel.IsAgentLevel())
            {
                return new OutstandingMemberCriteria(param.CustId, param.ProductIds);
            }

            if (param.CustLevel.IsMasterLevel())
            {
                return new OutstandingAgentCriteria(param.CustId, param.ProductIds, param.LevelType);
            }

            return new OutstandingMasterCriteria(param.CustId, param.ProductIds, param.LevelType);
        }
    }
}