namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Criteria.PlayForFun;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Models.PlayForFun;
    using Params;
    using RemotingRef;

    public interface PlayForFunService
    {
        IEnumerable<Product> SupportedProduct(int custId);

        IEnumerable<MasterAgentPlayForFunModel> MasterPlayForFun(PlayForFunParam param);

        IEnumerable<MasterAgentPlayForFunModel> AgentPlayForFun(PlayForFunParam param);

        IEnumerable<MemberPlayForFunModel> MemberPlayForFun(PlayForFunParam param);
    }

    public class PlayForFunServiceImpl : PlayForFunService
    {
        private const string PlayForFunProductsCacheKey = "Alpha.Reports.PlayForFunProducts";

        private readonly CacheRefService cacheRefService;
        private readonly IUser user;
        private readonly IDynamicRepository repository;

        public PlayForFunServiceImpl(
            CacheRefService cacheRefService,
            IUser user,
            IDynamicRepository dynamicRepository)
        {
            this.cacheRefService = cacheRefService;
            this.user = user;
            repository = dynamicRepository;
        }

        public IEnumerable<Product> SupportedProduct(int custId)
        {
            var playForFunProducts = ConfiguredCacheService.GetOrSet(
                $"PlayForFunProductsCacheKey-{custId}",
                () => repository.Fetch<PlayForFunProduct>(new PlayForFunProductCriteria(custId)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return playForFunProducts
                .Where(product => IsShowProduct(product.Id))
                .Select(product => new Product(product.Id, string.Empty, cacheRefService.GetProductName));
        }

        public bool IsShowProduct(int productId)
        {
            return productId != ProductId.FishingWorld || !user.GetHideProduct(productId);
        }

        public IEnumerable<MasterAgentPlayForFunModel> MasterPlayForFun(PlayForFunParam param)
        {
            var criteria = new MasterPlayForFunCriteria(param.CustId, param.FromDate, param.ToDate, param.ProductId, param.StatusId);
            var data = repository.Fetch<MasterAgentPlayForFunModel>(criteria);

            return data;
        }

        public IEnumerable<MasterAgentPlayForFunModel> AgentPlayForFun(PlayForFunParam param)
        {
            var criteria = new AgentPlayForFunCriteria(param.CustId, param.FromDate, param.ToDate, param.ProductId, param.StatusId);
            var data = repository.Fetch<MasterAgentPlayForFunModel>(criteria);

            return data;
        }

        public IEnumerable<MemberPlayForFunModel> MemberPlayForFun(PlayForFunParam param)
        {
            var criteria = new MemberPlayForFunCriteria(param.CustId, param.FromDate, param.ToDate, param.ProductId, param.StatusId);
            var data = repository.Fetch<MemberPlayForFunModel>(criteria);

            return data;
        }
    }
}
