namespace Alpha.Reports.Features.AutoFollowBets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Features.AutoFollowBets.Criteria;
    using Alpha.Reports.Features.AutoFollowBets.Models;
    using Fanex.Data.Repository;

    public interface AutoFollowBetService
    {
        IEnumerable<Tuple<int, IEnumerable<FollowBetAccount>>> GetFollowBetAccounts(int agentId);
    }

    public class AutoFollowBetServiceImpl : AutoFollowBetService
    {
        private readonly IDynamicRepository repository;

        public AutoFollowBetServiceImpl(IDynamicRepository dynamicRepository)
            => repository = dynamicRepository;

        public IEnumerable<Tuple<int, IEnumerable<FollowBetAccount>>> GetFollowBetAccounts(int agentId)
        {
            var followBetAccounts = repository.Fetch<FollowBetAccount>(new GetFollowBetAccountsCriteria(agentId));

            return followBetAccounts
                .GroupBy(f => f.AgentId, (key, accounts) => Tuple.Create(key, accounts));
        }
    }
}
