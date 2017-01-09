namespace Alpha.Reports.Services.Outstanding
{
    using System.Collections.Generic;
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
            if (userLevel >= custLevel)
            {
                var criteria = new OutstandingCriteria(custLevel, custId);

                return repository.Fetch<OutstandingData>(criteria);
            }

            return new List<OutstandingData>();
        }
    }
}
