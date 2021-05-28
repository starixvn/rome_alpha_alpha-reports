namespace Alpha.Reports.Services.Outstanding
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
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
        private readonly PartialUMService partialUMService;

        public OutstandingServiceImpl(IDynamicRepository dynamicRepository, PartialUMService partialUMService)
        {
            repository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<OutstandingModel> GetMemberOutstanding(OutstandingParam param)
        {
            if (param.CustId <= 0 || param.CustLevel.IsUplineOf(param.UserLevel))
            {
                return Enumerable.Empty<OutstandingModel>();
            }

            var criteria = new OutstandingCriteria(param.CustLevel, param.CustId, param.ProductIds);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return repository.Fetch<OutstandingModel>(criteria);
        }
    }
}