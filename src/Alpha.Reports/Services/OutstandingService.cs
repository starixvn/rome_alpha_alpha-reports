namespace Alpha.Reports.Services.Outstanding
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.Outstanding;
    using Fanex.Data.Repository;
    using Models;
    using Models.Outstanding;

    public interface OutstandingService
    {
        IEnumerable<OutstandingData> GetMemberOutstanding(UserLevel userLevel, UserLevel custLevel, int custId);
    }

    public class OutstandingServiceImpl : OutstandingService
    {
        private readonly IDynamicRepository repository;

        public OutstandingServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public IEnumerable<OutstandingData> GetMemberOutstanding(UserLevel userLevel, UserLevel custLevel, int custId)
        {
            if (custId <= 0 || custLevel > userLevel)
            {
                return Enumerable.Empty<OutstandingData>();
            }

            var criteria = new OutstandingCriteria(custLevel, custId);

            return repository.Fetch<OutstandingData>(criteria);
        }
    }
}
